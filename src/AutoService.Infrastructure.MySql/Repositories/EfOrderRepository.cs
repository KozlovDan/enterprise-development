using AutoService.Domain.Entities;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;
using AutoService.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.MySql.Repositories;

/// <summary>История и аналитика заказов; группировки и суммы выполняет MySQL.</summary>
/// <param name="context">Контекст текущего запроса.</param>
public class EfOrderRepository(AutoServiceDbContext context) : EfRepository<Order>(context), IOrderRepository
{
    /// <inheritdoc />
    protected override IQueryable<Order> Query => Context.Orders.Include(order => order.WorkTypes);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Client>> GetClientsByMechanicAsync(int mechanicId) =>
        await Context.Clients.AsNoTracking()
            .Where(client => Context.Orders.Any(order => order.MechanicId == mechanicId && order.ClientId == client.Id))
            .OrderBy(client => client.FullName).ThenBy(client => client.Id).ToListAsync();

    /// <inheritdoc />
    public async Task<IReadOnlyList<RepeatVisit>> GetRepeatVisitsAsync(DateTimeOffset reportDate)
    {
        var periodStart = reportDate.AddMonths(-1);
        return await Context.Orders.AsNoTracking()
            .Where(order => order.AcceptedAt >= periodStart && order.AcceptedAt < reportDate)
            .Where(order => Context.Orders.Any(previous => previous.ClientId == order.ClientId &&
                (previous.AcceptedAt < order.AcceptedAt || previous.AcceptedAt == order.AcceptedAt && previous.Id < order.Id)))
            .GroupBy(order => order.ClientId)
            .OrderBy(group => group.Key)
            .Select(group => new RepeatVisit(group.Key, group.Count())).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<WorkFrequency>> GetMostFrequentWorkTypesAsync() =>
        await Context.Orders.AsNoTracking().SelectMany(order => order.WorkTypes)
            .GroupBy(work => work.Id)
            .OrderByDescending(group => group.Count()).ThenBy(group => group.Key).Take(5)
            .Select(group => new WorkFrequency(group.Key, group.Count())).ToListAsync();

    /// <inheritdoc />
    public async Task<decimal> GetTotalCostAsync(int id) => await Context.Orders
        .Where(order => order.Id == id).Select(order => (decimal?)order.WorkTypes.Sum(work => work.Cost)).SingleOrDefaultAsync()
        ?? throw new EntityNotFoundException($"Запись с идентификатором {id} не найдена.");
}
