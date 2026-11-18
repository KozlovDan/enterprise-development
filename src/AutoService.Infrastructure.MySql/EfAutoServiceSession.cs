using System.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces;
using AutoService.Infrastructure.MySql.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.MySql;

/// <summary>Одна транзакция на проверку связей и изменение данных.</summary>
/// <param name="context">Контекст текущего HTTP-запроса.</param>
public class EfAutoServiceSession(AutoServiceDbContext context) : IAutoServiceSession
{
    /// <inheritdoc />
    public IRepository<Client> Clients { get; } = new EfRepository<Client>(context);
    /// <inheritdoc />
    public IRepository<Car> Cars { get; } = new EfRepository<Car>(context);
    /// <inheritdoc />
    public IRepository<Mechanic> Mechanics { get; } = new EfRepository<Mechanic>(context);
    /// <inheritdoc />
    public IRepository<WorkType> WorkTypes { get; } = new EfRepository<WorkType>(context);
    /// <inheritdoc />
    public IOrderRepository Orders { get; } = new EfOrderRepository(context);

    /// <inheritdoc />
    public Task<T> ExecuteAsync<T>(Func<Task<T>> action) => context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
    {
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var result = await action();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            context.ChangeTracker.Clear();
            throw;
        }
    });

    /// <inheritdoc />
    public async Task ExecuteAsync(Func<Task> action) => await ExecuteAsync(async () =>
    {
        await action();
        return true;
    });
}
