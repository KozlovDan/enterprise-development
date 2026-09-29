using System.Globalization;
using AutoService.Tests.Fixtures;

namespace AutoService.Tests;

/// <summary>Пять аналитических запросов из задания по предметной области «Автосервис».</summary>
/// <param name="fixture">Общие неизменяемые тестовые данные.</param>
public class QueryTests(AutoServiceFixture fixture) : IClassFixture<AutoServiceFixture>
{
    /// <summary>Эталонные механики для каждого вида работ, включая отсутствие специалистов.</summary>
    public static TheoryData<int, int[]> MechanicsByWorkTypeCases => new()
    {
        { 1, [1, 2] },
        { 2, [1, 2] },
        { 3, [3, 4] },
        { 4, [3, 4] },
        { 5, [5, 6] },
        { 6, [5, 6] },
        { 7, [7, 8] },
        { 8, [7, 8] },
        { 9, [9, 10] },
        { 10, [9, 10] },
        { 11, [] },
        { 12, [] }
    };

    /// <summary>Эталонные клиенты каждого механика в порядке ФИО, без повторов.</summary>
    public static TheoryData<int, int[]> ClientsByMechanicCases => new()
    {
        { 1, [6, 1] },
        { 2, [6, 1] },
        { 3, [2, 7] },
        { 4, [2, 7] },
        { 5, [8, 3] },
        { 6, [8, 3] },
        { 7, [4, 9] },
        { 8, [4] },
        { 9, [10, 5] },
        { 10, [5] },
        { 999, [] }
    };

    /// <summary>Даты отчёта, идентификаторы клиентов и точные количества повторных визитов.</summary>
    public static TheoryData<DateTimeOffset, int[], int[]> RepeatVisitsCases => new()
    {
        { new(2026, 7, 1, 12, 0, 0, TimeSpan.Zero), [], [] },
        { new(2026, 8, 27, 12, 0, 0, TimeSpan.Zero), [2], [1] },
        { new(2026, 9, 1, 12, 0, 0, TimeSpan.Zero), [1, 2], [1, 1] },
        { new(2026, 9, 27, 12, 0, 0, TimeSpan.Zero), [1, 2, 3, 4, 5, 6, 7, 8], [4, 3, 2, 2, 1, 1, 1, 1] },
        { new(2026, 9, 28, 12, 0, 0, TimeSpan.Zero), [1, 2, 3, 4, 5, 6, 7, 8], [3, 4, 2, 2, 1, 1, 1, 1] },
        { new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero), [3], [1] }
    };

    /// <summary>Стоимость каждого заказа, рассчитанная по исходному прейскуранту.</summary>
    public static TheoryData<int, decimal> OrderCostCases => new()
    {
        { 1, 1600.75m },
        { 2, 5300.00m },
        { 3, 2500.00m },
        { 4, 900.00m },
        { 5, 1200.50m },
        { 6, 3500.00m },
        { 7, 1600.75m },
        { 8, 5300.00m },
        { 9, 8500.00m },
        { 10, 2300.00m },
        { 11, 7000.00m },
        { 12, 1600.75m },
        { 13, 3500.00m },
        { 14, 2500.00m },
        { 15, 900.00m },
        { 16, 12000.00m },
        { 17, 1600.75m },
        { 18, 3500.00m },
        { 19, 5300.00m },
        { 20, 2500.00m },
        { 21, 900.00m },
        { 22, 7000.00m },
        { 23, 1200.50m },
        { 24, 5300.00m },
        { 25, 8500.00m },
        { 26, 1200.50m },
        { 27, 3500.00m },
        { 28, 2500.00m }
    };

    /// <summary>Полная история, короткая история и отсутствие заказов для рейтинга работ.</summary>
    public static TheoryData<int, int[], int[]> MostFrequentWorksCases => new()
    {
        { 28, [3, 1, 5, 2, 4], [8, 7, 6, 4, 4] },
        { 1, [1, 2], [1, 1] },
        { 0, [], [] }
    };

    /// <summary>Находит всех механиков, специализация которых соответствует выбранной работе.</summary>
    [Theory]
    [MemberData(nameof(MechanicsByWorkTypeCases))]
    public void GetMechanicsByWorkType(int workTypeId, int[] expectedIds)
    {
        // Arrange
        var workType = fixture.WorkTypes.Single(work => work.Id == workTypeId);

        // Act
        var actual = fixture.Mechanics
            .Where(mechanic => mechanic.Specialization == workType.Category)
            .OrderBy(mechanic => mechanic.Id)
            .ToArray();

        // Assert
        Assert.Equal(expectedIds, actual.Select(mechanic => mechanic.Id));
    }

    /// <summary>Возвращает клиентов выбранного механика один раз и сортирует их по ФИО.</summary>
    [Theory]
    [MemberData(nameof(ClientsByMechanicCases))]
    public void GetClientsByMechanic(int mechanicId, int[] expectedIds)
    {
        // Arrange
        var comparer = StringComparer.Create(CultureInfo.GetCultureInfo("ru-RU"), ignoreCase: false);

        // Act
        var actual = fixture.Orders
            .Where(order => order.Mechanic.Id == mechanicId)
            .Select(order => order.Client)
            .DistinctBy(client => client.Id)
            .OrderBy(client => client.FullName, comparer)
            .ThenBy(client => client.Id)
            .ToArray();

        // Assert
        Assert.Equal(expectedIds, actual.Select(client => client.Id));
    }

    /// <summary>
    /// Считает повторные визиты за интервал [дата отчёта минус месяц, дата отчёта).
    /// Первое обращение исключается из всей истории клиента до фильтрации по датам.
    /// </summary>
    [Theory]
    [MemberData(nameof(RepeatVisitsCases))]
    public void GetRepeatVisitsInLastMonth(
        DateTimeOffset reportDate,
        int[] expectedClientIds,
        int[] expectedCounts)
    {
        // Arrange
        var periodStart = reportDate.AddMonths(-1);

        // Act
        var actual = fixture.Orders
            .GroupBy(order => order.Client.Id)
            .SelectMany(group => group
                .OrderBy(order => order.AcceptedAt)
                .ThenBy(order => order.Id)
                .Skip(1))
            .Where(order => order.AcceptedAt >= periodStart && order.AcceptedAt < reportDate)
            .GroupBy(order => order.Client.Id)
            .OrderBy(group => group.Key)
            .Select(group => (ClientId: group.Key, Count: group.Count()))
            .ToArray();

        // Assert
        Assert.Equal(expectedClientIds, actual.Select(row => row.ClientId));
        Assert.Equal(expectedCounts, actual.Select(row => row.Count));
    }

    /// <summary>Суммирует стоимость всех работ выбранного заказа с точностью decimal.</summary>
    [Theory]
    [MemberData(nameof(OrderCostCases))]
    public void GetOrderTotalCost(int orderId, decimal expectedCost)
    {
        // Arrange
        var order = fixture.Orders.Single(order => order.Id == orderId);

        // Act
        var actual = order.WorkTypes.Sum(work => work.Cost);

        // Assert
        Assert.Equal(expectedCost, actual);
    }

    /// <summary>Находит пять самых частых работ; при равной частоте первым идёт меньший Id.</summary>
    [Theory]
    [MemberData(nameof(MostFrequentWorksCases))]
    public void GetMostFrequentWorkTypes(
        int orderCount,
        int[] expectedWorkTypeIds,
        int[] expectedCounts)
    {
        // Arrange
        var orders = fixture.Orders.Take(orderCount).ToArray();

        // Act
        var actual = orders
            .SelectMany(order => order.WorkTypes)
            .GroupBy(work => work.Id)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Take(5)
            .Select(group => (WorkTypeId: group.Key, Count: group.Count()))
            .ToArray();

        // Assert
        Assert.Equal(expectedWorkTypeIds, actual.Select(row => row.WorkTypeId));
        Assert.Equal(expectedCounts, actual.Select(row => row.Count));
    }
}
