using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AutoService.Tests.Api;

/// <summary>Проверки CRUD через настоящий конвейер ASP.NET Core.</summary>
public class CrudApiTests : ApiTestBase
{
    /// <summary>Полный цикл CRUD для каждой сущности, включая Location и непереиспользование Id.</summary>
    [Theory]
    [InlineData("clients", "fullName", "Тестовый клиент", "Обновлённый клиент")]
    [InlineData("cars", "model", "Vesta", "Granta")]
    [InlineData("mechanics", "fullName", "Тестовый механик", "Обновлённый механик")]
    [InlineData("work-types", "name", "Замена масла", "Проверка масла")]
    [InlineData("orders", "returnedAt", null, "2026-10-01T14:00:00+00:00")]
    public async Task Resource_SupportsFullCrudCycle(
        string resource, string field, string? before, string? after)
    {
        var url = $"/api/{resource}";
        using var created = await Client.PostAsJsonAsync(url, Request(resource), cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        var entity = await created.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: TestContext.Current.CancellationToken);
        var id = entity.GetProperty("id").GetInt32();
        Assert.Equal(before, entity.GetProperty(field).GetString());
        Assert.NotNull(created.Headers.Location);
        Assert.EndsWith($"{url}/{id}", created.Headers.Location.ToString());
        using var read = await Client.GetAsync(created.Headers.Location, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, read.StatusCode);

        using var updated = await Client.PutAsJsonAsync($"{url}/{id}", Request(resource, updated: true), cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NoContent, updated.StatusCode);
        var saved = await Client.GetFromJsonAsync<JsonElement>($"{url}/{id}", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(after, saved.GetProperty(field).GetString());

        using var deleted = await Client.DeleteAsync($"{url}/{id}", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NoContent, deleted.StatusCode);
        using var missing = await Client.GetAsync($"{url}/{id}", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
        using var next = await Client.PostAsJsonAsync(url, Request(resource), cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, next.StatusCode);
        var nextEntity = await next.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(nextEntity.GetProperty("id").GetInt32() > id);
    }

    /// <summary>Отсутствующий ресурс даёт 404 для чтения, изменения и удаления.</summary>
    [Theory]
    [InlineData("clients")]
    [InlineData("cars")]
    [InlineData("mechanics")]
    [InlineData("work-types")]
    [InlineData("orders")]
    public async Task MissingResource_ReturnsNotFound(string resource)
    {
        var url = $"/api/{resource}/99999";
        using var get = await Client.GetAsync(url, cancellationToken: TestContext.Current.CancellationToken);
        using var put = await Client.PutAsJsonAsync(url, Request(resource), cancellationToken: TestContext.Current.CancellationToken);
        using var delete = await Client.DeleteAsync(url, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);
    }

    /// <summary>Связанные справочные записи нельзя удалять.</summary>
    [Theory]
    [InlineData("clients")]
    [InlineData("cars")]
    [InlineData("mechanics")]
    [InlineData("work-types")]
    public async Task ReferencedResource_DeleteReturnsConflict(string resource)
    {
        using var response = await Client.DeleteAsync($"/api/{resource}/1", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        using var read = await Client.GetAsync($"/api/{resource}/1", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, read.StatusCode);
    }

    /// <summary>Параллельные запросы не теряют записи и получают уникальные Id.</summary>
    [Fact]
    public async Task ParallelCreates_AssignUniqueIds()
    {
        var responses = await Task.WhenAll(Enumerable.Range(0, 30)
            .Select(_ => Client.PostAsJsonAsync("/api/clients", Request("clients"), cancellationToken: TestContext.Current.CancellationToken)));
        List<int> ids = [];
        foreach (var response in responses)
        {
            using (response)
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                var entity = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: TestContext.Current.CancellationToken);
                ids.Add(entity.GetProperty("id").GetInt32());
            }
        }
        Assert.Equal(30, ids.Distinct().Count());
        var clients = await Client.GetFromJsonAsync<JsonElement>("/api/clients", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(40, clients.GetArrayLength());
    }

    private static object Request(string resource, bool updated = false) => resource switch
    {
        "clients" => new
        {
            fullName = updated ? "Обновлённый клиент" : "Тестовый клиент",
            phone = "+79991234567"
        },
        "cars" => new
        {
            registrationNumber = "А999АА163",
            brand = "Lada",
            model = updated ? "Granta" : "Vesta",
            manufactureYear = 2020,
            ownerId = 1
        },
        "mechanics" => new
        {
            fullName = updated ? "Обновлённый механик" : "Тестовый механик",
            passportNumber = "1111 999999",
            specialization = "Tires",
            experienceYears = 2
        },
        "work-types" => new
        {
            name = updated ? "Проверка масла" : "Замена масла",
            category = "Maintenance",
            cost = 100.25m,
            durationMinutes = 10
        },
        "orders" => new
        {
            carId = 1,
            clientId = 1,
            mechanicId = 1,
            workTypeIds = new[] { 1, 2 },
            acceptedAt = "2026-10-01T12:00:00Z",
            returnedAt = updated ? "2026-10-01T14:00:00Z" : null
        },
        _ => throw new ArgumentOutOfRangeException(nameof(resource))
    };
}
