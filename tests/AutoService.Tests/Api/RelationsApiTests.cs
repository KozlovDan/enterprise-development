using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoService.Contracts.Dtos;

namespace AutoService.Tests.Api;

/// <summary>Регрессии для связей между справочниками и заказами.</summary>
public class RelationsApiTests : ApiTestBase
{
    /// <summary>Все маршруты связанных ресурсов возвращают существующие записи.</summary>
    [Theory]
    [InlineData("clients/1/cars")]
    [InlineData("clients/1/orders")]
    [InlineData("cars/1/owner")]
    [InlineData("cars/1/orders")]
    [InlineData("mechanics/1/orders")]
    [InlineData("work-types/1/orders")]
    [InlineData("orders/1/car")]
    [InlineData("orders/1/client")]
    [InlineData("orders/1/mechanic")]
    [InlineData("orders/1/work-types")]
    public async Task RelatedResource_IsAvailable(string path)
    {
        using var response = await Client.GetAsync($"/api/{path}", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>Переименование клиента видно в ранее созданном заказе.</summary>
    [Fact]
    public async Task ClientUpdate_ChangesExistingOrderReference()
    {
        using var response = await Client.PutAsJsonAsync("/api/clients/1", new
        {
            fullName = "Новое имя клиента",
            phone = "+79991234567"
        }, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var customer = await Client.GetFromJsonAsync<ClientDto>("/api/orders/1/client", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(customer);
        Assert.Equal("Новое имя клиента", customer.FullName);
    }

    /// <summary>Изменение цены учитывается при расчёте стоимости существующего заказа.</summary>
    [Fact]
    public async Task WorkTypeUpdate_ChangesExistingOrderCost()
    {
        using var response = await Client.PutAsJsonAsync("/api/work-types/1", new
        {
            name = "Замена моторного масла",
            category = "Maintenance",
            cost = 2000.25m,
            durationMinutes = 40
        }, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var total = await Client.GetFromJsonAsync<OrderCostDto>("/api/orders/1/total-cost", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(total);
        Assert.Equal(2400.50m, total.TotalCost);
    }

    /// <summary>Смена владельца не переписывает историю заказов предыдущего клиента.</summary>
    [Fact]
    public async Task OwnerChange_PreservesHistoricalCustomer()
    {
        using var response = await Client.PutAsJsonAsync("/api/cars/1", new
        {
            registrationNumber = "А101АА163",
            brand = "Lada",
            model = "Vesta",
            manufactureYear = 2020,
            ownerId = 2
        }, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var car = await Client.GetFromJsonAsync<CarDto>("/api/orders/1/car", cancellationToken: TestContext.Current.CancellationToken);
        var customer = await Client.GetFromJsonAsync<ClientDto>("/api/orders/1/client", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(car);
        Assert.NotNull(customer);
        Assert.Equal(2, car.OwnerId);
        Assert.Equal(1, customer.Id);

        using var completed = await Client.PutAsJsonAsync("/api/orders/1", new
        {
            carId = 1,
            clientId = 1,
            mechanicId = 1,
            workTypeIds = new[] { 1, 2 },
            acceptedAt = "2026-06-01T12:00:00Z",
            returnedAt = "2026-06-01T14:00:00Z"
        }, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NoContent, completed.StatusCode);
    }

    /// <summary>Изменение специализации или категории не ломает заказы; отказ не меняет поля.</summary>
    [Theory]
    [InlineData("mechanics", "{\"fullName\":\"Новое имя\",\"passportNumber\":\"0000 000001\",\"specialization\":\"Tires\",\"experienceYears\":12}")]
    [InlineData("work-types", "{\"name\":\"Новое имя\",\"category\":\"Tires\",\"cost\":2000,\"durationMinutes\":40}")]
    public async Task IncompatibleUpdate_IsRejectedWithoutPartialChanges(string resource, string json)
    {
        var path = $"/api/{resource}/1";
        var before = await Client.GetStringAsync(path, cancellationToken: TestContext.Current.CancellationToken);
        using var body = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        using var response = await Client.PutAsync(path, body, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal(before, await Client.GetStringAsync(path, cancellationToken: TestContext.Current.CancellationToken));
    }

    /// <summary>Госномер и паспорт уникальны даже при добавлении существующей записи.</summary>
    [Theory]
    [InlineData("cars", "{\"registrationNumber\":\"а101аа163\",\"brand\":\"Lada\",\"model\":\"Vesta\",\"manufactureYear\":2020,\"ownerId\":1}")]
    [InlineData("mechanics", "{\"fullName\":\"Иван\",\"passportNumber\":\"0000 000001\",\"specialization\":\"Tires\",\"experienceYears\":1}")]
    public async Task DuplicateUniqueField_ReturnsConflict(string resource, string json)
    {
        using var body = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        using var response = await Client.PostAsync($"/api/{resource}", body, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    /// <summary>После удаления всех заказов рейтинг возвращает пустой массив.</summary>
    [Fact]
    public async Task EmptyOrderHistory_HasEmptyAnalytics()
    {
        var orders = await Client.GetFromJsonAsync<JsonElement>("/api/orders", cancellationToken: TestContext.Current.CancellationToken);
        foreach (var order in orders.EnumerateArray())
        {
            using var response = await Client.DeleteAsync($"/api/orders/{order.GetProperty("id").GetInt32()}", cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        var result = await Client.GetFromJsonAsync<WorkFrequencyDto[]>("/api/analytics/most-frequent-work-types", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
