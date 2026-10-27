using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AutoService.Tests.Api;

/// <summary>Некорректный ввод и нарушения связей должны возвращать 400 без изменения данных.</summary>
public class ValidationApiTests : ApiTestBase
{
    /// <summary>Обязательные поля, перечисления, числа, даты и ссылки проверяются сервером.</summary>
    [Theory]
    [InlineData("clients", "{}")]
    [InlineData("cars", "{}")]
    [InlineData("mechanics", "{}")]
    [InlineData("work-types", "{}")]
    [InlineData("orders", "{}")]
    [InlineData("clients", "{\"fullName\":\"   \",\"phone\":\"+79001234567\"}")]
    [InlineData("clients", "{\"fullName\":\"Иван\",\"phone\":\"abc\"}")]
    [InlineData("cars", "{\"registrationNumber\":\"TEST\",\"brand\":\"Lada\",\"model\":\"Vesta\",\"manufactureYear\":9999,\"ownerId\":1}")]
    [InlineData("cars", "{\"registrationNumber\":\"TEST\",\"brand\":\"Lada\",\"model\":\"Vesta\",\"manufactureYear\":2020,\"ownerId\":99999}")]
    [InlineData("mechanics", "{\"fullName\":\"Иван\",\"passportNumber\":\"1234 123456\",\"experienceYears\":2}")]
    [InlineData("mechanics", "{\"fullName\":\"Иван\",\"passportNumber\":\"1234 123456\",\"specialization\":\"Unknown\",\"experienceYears\":2}")]
    [InlineData("mechanics", "{\"fullName\":\"Иван\",\"passportNumber\":\"1234 123456\",\"specialization\":0,\"experienceYears\":2}")]
    [InlineData("mechanics", "{\"fullName\":\"Иван\",\"passportNumber\":\"1234 123456\",\"specialization\":\"Tires\",\"experienceYears\":-1}")]
    [InlineData("work-types", "{\"name\":\"Работа\",\"category\":\"Tires\",\"cost\":-1,\"durationMinutes\":10}")]
    [InlineData("work-types", "{\"name\":\"Работа\",\"category\":\"Tires\",\"cost\":10,\"durationMinutes\":-10}")]
    [InlineData("work-types", "{\"name\":\"Работа\",\"cost\":10,\"durationMinutes\":10}")]
    public async Task InvalidRequest_ReturnsValidationProblem(string resource, string json)
    {
        using var body = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await Client.PostAsync($"/api/{resource}", body, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(400, problem.GetProperty("status").GetInt32());
        Assert.True(problem.TryGetProperty("errors", out _));
    }

    /// <summary>Заказ не допускает чужую машину, неподходящего механика, неизвестные и повторные работы.</summary>
    [Theory]
    [InlineData(2, 1, "1,2", "2026-10-01T14:00:00Z")]
    [InlineData(1, 3, "1,2", "2026-10-01T14:00:00Z")]
    [InlineData(1, 1, "99999", "2026-10-01T14:00:00Z")]
    [InlineData(1, 1, "1,1", "2026-10-01T14:00:00Z")]
    [InlineData(1, 1, "0", "2026-10-01T14:00:00Z")]
    [InlineData(1, 1, "", "2026-10-01T14:00:00Z")]
    [InlineData(1, 1, "1", "2026-10-01T11:00:00Z")]
    public async Task InvalidOrder_DoesNotOverwriteExistingOrder(
        int clientId, int mechanicId, string workIds, string returnedAt)
    {
        var before = await Client.GetStringAsync("/api/orders/1", cancellationToken: TestContext.Current.CancellationToken);
        var ids = workIds.Length == 0 ? [] : workIds.Split(',').Select(int.Parse).ToArray();
        using var response = await Client.PutAsJsonAsync("/api/orders/1", new
        {
            carId = 1,
            clientId,
            mechanicId,
            workTypeIds = ids,
            acceptedAt = "2026-10-01T12:00:00Z",
            returnedAt
        }, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(before, await Client.GetStringAsync("/api/orders/1", cancellationToken: TestContext.Current.CancellationToken));
    }

    /// <summary>Отсутствующая, некорректная и слишком ранняя дата отчёта дают 400.</summary>
    [Theory]
    [InlineData("")]
    [InlineData("?reportDate=invalid")]
    [InlineData("?reportDate=0001-01-01T00:00:00Z")]
    public async Task InvalidReportDate_ReturnsBadRequest(string query)
    {
        using var response = await Client.GetAsync($"/api/analytics/repeat-visits{query}", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
