using System.Globalization;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces;
using AutoService.Domain.Queries;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>Запросы к истории заказов в памяти.</summary>
/// <param name="orders">Начальные заказы.</param>
public class OrderRepository(IEnumerable<Order> orders) : InMemoryRepository<Order>(orders), IOrderRepository
{
    /// <inheritdoc />
    public IReadOnlyList<Client> GetClientsByMechanic(int mechanicId)
    {
        var comparer = StringComparer.Create(CultureInfo.GetCultureInfo("ru-RU"), ignoreCase: false);
        return [.. Find(order => order.Mechanic.Id == mechanicId)
            .Select(order => order.Client)
            .DistinctBy(client => client.Id)
            .OrderBy(client => client.FullName, comparer)
            .ThenBy(client => client.Id)];
    }

    /// <inheritdoc />
    public IReadOnlyList<RepeatVisit> GetRepeatVisits(DateTimeOffset reportDate)
    {
        var periodStart = reportDate.AddMonths(-1);
        return [.. GetAll()
            .GroupBy(order => order.Client.Id)
            .SelectMany(group => group.OrderBy(order => order.AcceptedAt).ThenBy(order => order.Id).Skip(1))
            .Where(order => order.AcceptedAt >= periodStart && order.AcceptedAt < reportDate)
            .GroupBy(order => order.Client.Id)
            .OrderBy(group => group.Key)
            .Select(group => new RepeatVisit(group.Key, group.Count()))];
    }

    /// <inheritdoc />
    public IReadOnlyList<WorkFrequency> GetMostFrequentWorkTypes() =>
        [.. GetAll().SelectMany(order => order.WorkTypes)
            .GroupBy(work => work.Id)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Take(5)
            .Select(group => new WorkFrequency(group.Key, group.Count()))];
}
