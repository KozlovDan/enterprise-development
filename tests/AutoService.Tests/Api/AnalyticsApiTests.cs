using System.Net.Http.Json;
using AutoService.Contracts.Dtos;

namespace AutoService.Tests.Api;

/// <summary>Те же эталоны, что в первой работе, проверяются через серверный API.</summary>
public class AnalyticsApiTests : ApiTestBase
{
    /// <summary>Соответствие специализации каждому виду работ, включая пустой результат.</summary>
    [Theory]
    [MemberData(nameof(QueryTests.MechanicsByWorkTypeCases), MemberType = typeof(QueryTests))]
    public async Task GetMechanics_MatchesExpected(int workTypeId, int[] expectedIds)
    {
        var result = await Client.GetFromJsonAsync<MechanicDto[]>($"/api/work-types/{workTypeId}/mechanics", options: JsonOptions, cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(result);
        Assert.Equal(expectedIds, result.Select(mechanic => mechanic.Id));
    }

    /// <summary>Клиенты существующего механика сортируются по ФИО и не дублируются.</summary>
    [Theory]
    [InlineData(1, new[] { 6, 1 })]
    [InlineData(3, new[] { 2, 7 })]
    [InlineData(7, new[] { 4, 9 })]
    public async Task GetMechanicClients_MatchesExpected(int mechanicId, int[] expectedIds)
    {
        var result = await Client.GetFromJsonAsync<ClientDto[]>($"/api/mechanics/{mechanicId}/clients", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(result);
        Assert.Equal(expectedIds, result.Select(client => client.Id));
    }

    /// <summary>Проверяет границы месяца и исключение первого обращения из полной истории.</summary>
    [Theory]
    [MemberData(nameof(QueryTests.RepeatVisitsCases), MemberType = typeof(QueryTests))]
    public async Task GetRepeatVisits_MatchesExpected(
        DateTimeOffset reportDate, int[] expectedClientIds, int[] expectedCounts)
    {
        var date = Uri.EscapeDataString(reportDate.ToString("O"));
        var result = await Client.GetFromJsonAsync<RepeatVisitDto[]>($"/api/analytics/repeat-visits?reportDate={date}", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(result);
        Assert.Equal(expectedClientIds, result.Select(row => row.ClientId));
        Assert.Equal(expectedCounts, result.Select(row => row.Count));
    }

    /// <summary>Стоимость всех исходных заказов сохраняет точность decimal.</summary>
    [Theory]
    [MemberData(nameof(QueryTests.OrderCostCases), MemberType = typeof(QueryTests))]
    public async Task GetOrderTotalCost_MatchesExpected(int orderId, decimal expectedCost)
    {
        var result = await Client.GetFromJsonAsync<OrderCostDto>($"/api/orders/{orderId}/total-cost", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(expectedCost, result.TotalCost);
    }

    /// <summary>Топ-5 упорядочивается по частоте и затем по идентификатору.</summary>
    [Fact]
    public async Task GetMostFrequentWorkTypes_MatchesExpected()
    {
        var result = await Client.GetFromJsonAsync<WorkFrequencyDto[]>("/api/analytics/most-frequent-work-types", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(result);
        Assert.Equal([3, 1, 5, 2, 4], result.Select(row => row.WorkTypeId));
        Assert.Equal([8, 7, 6, 4, 4], result.Select(row => row.Count));
    }
}
