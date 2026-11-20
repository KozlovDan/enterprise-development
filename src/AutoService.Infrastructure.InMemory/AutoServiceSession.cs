using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces;
using AutoService.Infrastructure.InMemory.Repositories;

namespace AutoService.Infrastructure.InMemory;

/// <summary>Общее хранилище с блокировкой на время операции сервиса.</summary>
public class AutoServiceSession : IAutoServiceSession
{
    private readonly SemaphoreSlim _gate = new(1, 1);

    /// <summary>Создаёт независимый набор связанных коллекций.</summary>
    public AutoServiceSession()
    {
        var seed = new AutoServiceSeed();
        Clients = new InMemoryRepository<Client>(seed.Clients);
        Cars = new InMemoryRepository<Car>(seed.Cars);
        Mechanics = new InMemoryRepository<Mechanic>(seed.Mechanics);
        WorkTypes = new InMemoryRepository<WorkType>(seed.WorkTypes);
        Orders = new OrderRepository(seed.Orders);
    }

    /// <inheritdoc />
    public IRepository<Client> Clients { get; }
    /// <inheritdoc />
    public IRepository<Car> Cars { get; }
    /// <inheritdoc />
    public IRepository<Mechanic> Mechanics { get; }
    /// <inheritdoc />
    public IRepository<WorkType> WorkTypes { get; }
    /// <inheritdoc />
    public IOrderRepository Orders { get; }

    /// <inheritdoc />
    public Task<T> ExecuteReadAsync<T>(Func<Task<T>> action) => ExecuteWriteAsync(action);

    /// <inheritdoc />
    public async Task<T> ExecuteWriteAsync<T>(Func<Task<T>> action)
    {
        // Блокировка охватывает всю операцию, в том числе асинхронные ожидания.
        await _gate.WaitAsync();
        try
        {
            return await action();
        }
        finally
        {
            _gate.Release();
        }
    }

    /// <inheritdoc />
    public async Task ExecuteWriteAsync(Func<Task> action)
    {
        await _gate.WaitAsync();
        try
        {
            await action();
        }
        finally
        {
            _gate.Release();
        }
    }
}
