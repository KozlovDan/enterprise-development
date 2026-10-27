using AutoService.Contracts.Dtos;

namespace AutoService.Application.Interfaces;

/// <summary>Операции с заказами.</summary>
public interface IOrderService : ICrudService<OrderRequest, OrderDto>
{
    /// <summary>Возвращает автомобиль заказа.</summary>
    public CarDto GetCar(int id);
    /// <summary>Возвращает клиента заказа.</summary>
    public ClientDto GetClient(int id);
    /// <summary>Возвращает механика заказа.</summary>
    public MechanicDto GetMechanic(int id);
    /// <summary>Возвращает виды работ заказа.</summary>
    public IReadOnlyList<WorkTypeDto> GetWorkTypes(int id);
    /// <summary>Рассчитывает суммарную стоимость заказа.</summary>
    public OrderCostDto GetTotalCost(int id);
}
