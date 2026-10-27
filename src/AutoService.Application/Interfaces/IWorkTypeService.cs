using AutoService.Contracts.Dtos;

namespace AutoService.Application.Interfaces;

/// <summary>Операции с видами работ.</summary>
public interface IWorkTypeService : ICrudService<WorkTypeRequest, WorkTypeDto>
{
    /// <summary>Возвращает заказы с данным видом работ.</summary>
    public IReadOnlyList<OrderDto> GetOrders(int id);
    /// <summary>Механики с подходящей специализацией.</summary>
    public IReadOnlyList<MechanicDto> GetMechanics(int id);
}
