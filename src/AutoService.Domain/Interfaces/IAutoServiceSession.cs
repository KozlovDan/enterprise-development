using AutoService.Domain.Entities;

namespace AutoService.Domain.Interfaces;

/// <summary>Репозитории автосервиса и выполнение операций чтения и изменения данных.</summary>
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
    /// <summary>Выполняет запросы и формирует ответ без открытия транзакции базы данных.</summary>
    public Task<T> ExecuteReadAsync<T>(Func<Task<T>> action);

    /// <summary>Выполняет проверку связей и изменение данных атомарно, возвращая результат операции.</summary>
    public Task<T> ExecuteWriteAsync<T>(Func<Task<T>> action);

    /// <summary>Выполняет проверку связей и изменение данных атомарно, без возвращаемого результата.</summary>
    public Task ExecuteWriteAsync(Func<Task> action);
}
