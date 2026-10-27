using AutoService.Contracts.Dtos;

namespace AutoService.Application.Interfaces;

/// <summary>Операции с автомобилями.</summary>
public interface ICarService : ICrudService<CarRequest, CarDto>
{
    /// <summary>Возвращает заказы автомобиля.</summary>
    public IReadOnlyList<OrderDto> GetOrders(int id);
    /// <summary>Возвращает владельца автомобиля.</summary>
    public ClientDto GetOwner(int id);
}
