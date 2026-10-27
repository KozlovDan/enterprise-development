using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces;
using AutoService.Infrastructure.InMemory.Repositories;

namespace AutoService.Infrastructure.InMemory;

/// <summary>Общее хранилище с блокировкой на время операции сервиса.</summary>
public class AutoServiceSession : IAutoServiceSession
{
    private readonly object _gate = new();

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
    public T Execute<T>(Func<T> action)
    {
        lock (_gate)
        {
            return action();
        }
    }

    /// <inheritdoc />
    public void Execute(Action action)
    {
        lock (_gate)
        {
            action();
        }
    }
}
