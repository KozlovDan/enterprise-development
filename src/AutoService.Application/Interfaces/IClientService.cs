using AutoService.Contracts.Dtos;

namespace AutoService.Application.Interfaces;

/// <summary>Операции с клиентами.</summary>
public interface IClientService : ICrudService<ClientRequest, ClientDto>
{
    /// <summary>Возвращает автомобили клиента.</summary>
    public IReadOnlyList<CarDto> GetCars(int id);
    /// <summary>Возвращает заказы клиента.</summary>
    public IReadOnlyList<OrderDto> GetOrders(int id);
}
