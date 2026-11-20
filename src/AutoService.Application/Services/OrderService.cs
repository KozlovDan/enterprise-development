using AutoService.Application.Interfaces;
using AutoService.Application.Mapping;
using AutoService.Contracts.Dtos;
using AutoService.Domain.Entities;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;

namespace AutoService.Application.Services;

/// <summary>Работа с заказами и проверка связанных данных.</summary>
/// <param name="session">Сеанс хранилища.</param>
public class OrderService(IAutoServiceSession session)
    : CrudService<Order, OrderRequest, OrderDto>(session, session.Orders), IOrderService
{
    /// <inheritdoc />
    protected override async Task<Order> BuildAsync(OrderRequest request, int id)
    {
        var car = await ReferenceAsync(Session.Cars, request.CarId!.Value, "Автомобиль");
        var client = await ReferenceAsync(Session.Clients, request.ClientId!.Value, "Клиент");
        var mechanic = await ReferenceAsync(Session.Mechanics, request.MechanicId!.Value, "Механик");
        var works = await Session.WorkTypes.GetByIdsAsync(request.WorkTypeIds);
        if (works.Count != request.WorkTypeIds.Count)
        {
            throw new DomainValidationException("Один или несколько видов работ не существуют.");
        }
        var original = await Session.Orders.GetByIdAsync(id);
        // После продажи автомобиля клиент в истории заказа остаётся прежним.
        var keepsCustomer = original?.CarId == car.Id && original.ClientId == client.Id;
        if (car.OwnerId != client.Id && !keepsCustomer)
        {
            throw new DomainValidationException("Новый заказ должен оформлять владелец автомобиля.");
        }
        if (works.Any(work => work.Category != mechanic.Specialization))
        {
            throw new DomainValidationException("Специализация механика не соответствует выбранным работам.");
        }
        return new()
        {
            Id = id,
            CarId = car.Id,
            Car = car,
            ClientId = client.Id,
            Client = client,
            MechanicId = mechanic.Id,
            Mechanic = mechanic,
            WorkTypes = works.ToList(),
            AcceptedAt = request.AcceptedAt!.Value,
            ReturnedAt = request.ReturnedAt
        };
    }

    /// <inheritdoc />
    protected override void Copy(Order source, Order target)
    {
        target.CarId = source.CarId;
        target.Car = source.Car;
        target.ClientId = source.ClientId;
        target.Client = source.Client;
        target.MechanicId = source.MechanicId;
        target.Mechanic = source.Mechanic;
        target.WorkTypes = source.WorkTypes;
        target.AcceptedAt = source.AcceptedAt;
        target.ReturnedAt = source.ReturnedAt;
    }

    /// <inheritdoc />
    protected override OrderDto ToDto(Order entity) => entity.ToDto();

    /// <inheritdoc />
    protected override Task CheckDeleteAsync(Order entity) => Task.CompletedTask;

    /// <inheritdoc />
    public Task<CarDto> GetCarAsync(int id) => Session.ExecuteReadAsync(async () =>
    {
        var order = await RequireAsync(Session.Orders, id);
        return (await RequireAsync(Session.Cars, order.CarId)).ToDto();
    });
    /// <inheritdoc />
    public Task<ClientDto> GetClientAsync(int id) => Session.ExecuteReadAsync(async () =>
    {
        var order = await RequireAsync(Session.Orders, id);
        return (await RequireAsync(Session.Clients, order.ClientId)).ToDto();
    });
    /// <inheritdoc />
    public Task<MechanicDto> GetMechanicAsync(int id) => Session.ExecuteReadAsync(async () =>
    {
        var order = await RequireAsync(Session.Orders, id);
        return (await RequireAsync(Session.Mechanics, order.MechanicId)).ToDto();
    });
    /// <inheritdoc />
    public Task<IReadOnlyList<WorkTypeDto>> GetWorkTypesAsync(int id) => Session.ExecuteReadAsync<IReadOnlyList<WorkTypeDto>>(async () =>
        [.. (await RequireAsync(Session.Orders, id)).WorkTypes.Select(work => work.ToDto())]);
    /// <inheritdoc />
    public Task<OrderCostDto> GetTotalCostAsync(int id) => Session.ExecuteReadAsync(async () =>
        new OrderCostDto(id, await Session.Orders.GetTotalCostAsync(id)));
}
