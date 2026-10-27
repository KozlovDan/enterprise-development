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
    protected override Order Build(OrderRequest request, int id)
    {
        var car = Reference(Session.Cars, request.CarId!.Value, "Автомобиль");
        var client = Reference(Session.Clients, request.ClientId!.Value, "Клиент");
        var mechanic = Reference(Session.Mechanics, request.MechanicId!.Value, "Механик");
        var works = Session.WorkTypes.GetByIds(request.WorkTypeIds);
        if (works.Count != request.WorkTypeIds.Count)
        {
            throw new DomainValidationException("Один или несколько видов работ не существуют.");
        }
        var original = Session.Orders.GetById(id);
        // После продажи автомобиля клиент в истории заказа остаётся прежним.
        var keepsCustomer = original?.Car.Id == car.Id && original.Client.Id == client.Id;
        if (car.Owner.Id != client.Id && !keepsCustomer)
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
            Car = car,
            Client = client,
            Mechanic = mechanic,
            WorkTypes = works,
            AcceptedAt = request.AcceptedAt!.Value,
            ReturnedAt = request.ReturnedAt
        };
    }

    /// <inheritdoc />
    protected override void Copy(Order source, Order target)
    {
        target.Car = source.Car;
        target.Client = source.Client;
        target.Mechanic = source.Mechanic;
        target.WorkTypes = source.WorkTypes;
        target.AcceptedAt = source.AcceptedAt;
        target.ReturnedAt = source.ReturnedAt;
    }

    /// <inheritdoc />
    protected override OrderDto ToDto(Order entity) => entity.ToDto();

    /// <inheritdoc />
    protected override void CheckDelete(Order entity)
    {
    }

    /// <inheritdoc />
    public CarDto GetCar(int id) => Session.Execute(() => Require(Session.Orders, id).Car.ToDto());
    /// <inheritdoc />
    public ClientDto GetClient(int id) => Session.Execute(() => Require(Session.Orders, id).Client.ToDto());
    /// <inheritdoc />
    public MechanicDto GetMechanic(int id) => Session.Execute(() => Require(Session.Orders, id).Mechanic.ToDto());
    /// <inheritdoc />
    public IReadOnlyList<WorkTypeDto> GetWorkTypes(int id) => Session.Execute<IReadOnlyList<WorkTypeDto>>(() =>
        [.. Require(Session.Orders, id).WorkTypes.Select(work => work.ToDto())]);
    /// <inheritdoc />
    public OrderCostDto GetTotalCost(int id) => Session.Execute(() =>
    {
        var order = Require(Session.Orders, id);
        return new OrderCostDto(order.Id, order.WorkTypes.Sum(work => work.Cost));
    });
}
