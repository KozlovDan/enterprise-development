using AutoService.Domain.Entities;

namespace AutoService.Domain.Interfaces;

/// <summary>Согласованный доступ к связанным коллекциям автосервиса.</summary>
public interface IAutoServiceSession
{
    /// <summary>Клиенты.</summary>
    public IRepository<Client> Clients { get; }
    /// <summary>Автомобили.</summary>
    public IRepository<Car> Cars { get; }
    /// <summary>Механики.</summary>
    public IRepository<Mechanic> Mechanics { get; }
    /// <summary>Прейскурант.</summary>
    public IRepository<WorkType> WorkTypes { get; }
    /// <summary>Заказы.</summary>
    public IOrderRepository Orders { get; }
    /// <summary>Выполняет операцию целиком, включая проверку связей и формирование результата.</summary>
    public T Execute<T>(Func<T> action);

    /// <summary>Выполняет изменение связанных данных как одну операцию.</summary>
    public void Execute(Action action);
}
