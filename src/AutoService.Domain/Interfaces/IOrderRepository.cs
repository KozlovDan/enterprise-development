using AutoService.Domain.Entities;
using AutoService.Domain.Queries;

namespace AutoService.Domain.Interfaces;

/// <summary>История заказов и аналитические запросы по ней.</summary>
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>Клиенты механика без повторов, упорядоченные по ФИО.</summary>
    public IReadOnlyList<Client> GetClientsByMechanic(int mechanicId);

    /// <summary>Повторные обращения за месяц до даты отчёта, не включая саму дату.</summary>
    public IReadOnlyList<RepeatVisit> GetRepeatVisits(DateTimeOffset reportDate);

    /// <summary>Пять самых частых видов работ с разрешением равенства по Id.</summary>
    public IReadOnlyList<WorkFrequency> GetMostFrequentWorkTypes();
}
