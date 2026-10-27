using AutoService.Application.Interfaces;
using AutoService.Application.Mapping;
using AutoService.Contracts.Dtos;
using AutoService.Domain.Entities;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;

namespace AutoService.Application.Services;

/// <summary>Работа с автомобилями и проверка связанных данных.</summary>
/// <param name="session">Сеанс хранилища.</param>
/// <param name="clock">Источник текущего года.</param>
public class CarService(IAutoServiceSession session, TimeProvider clock)
    : CrudService<Car, CarRequest, CarDto>(session, session.Cars), ICarService
{
    /// <inheritdoc />
    protected override Car Build(CarRequest request, int id)
    {
        var owner = Reference(Session.Clients, request.OwnerId!.Value, "Клиент");
        var number = request.RegistrationNumber.Trim().ToUpperInvariant();
        if (request.ManufactureYear > clock.GetUtcNow().Year)
        {
            throw new DomainValidationException("Год выпуска не может быть позже текущего года.");
        }
        if (Session.Cars.Exists(car => car.Id != id && car.RegistrationNumber == number))
        {
            throw new EntityConflictException("Автомобиль с таким регистрационным номером уже существует.");
        }
        return new()
        {
            Id = id,
            RegistrationNumber = number,
            Brand = request.Brand.Trim(),
            Model = request.Model.Trim(),
            ManufactureYear = request.ManufactureYear!.Value,
            Owner = owner
        };
    }

    /// <inheritdoc />
    protected override void Copy(Car source, Car target)
    {
        target.RegistrationNumber = source.RegistrationNumber;
        target.Brand = source.Brand;
        target.Model = source.Model;
        target.ManufactureYear = source.ManufactureYear;
        target.Owner = source.Owner;
    }

    /// <inheritdoc />
    protected override CarDto ToDto(Car entity) => entity.ToDto();

    /// <inheritdoc />
    protected override void CheckDelete(Car entity)
    {
        if (Session.Orders.Exists(order => order.Car.Id == entity.Id))
        {
            throw new EntityConflictException("Запись используется. Сначала удалите связанные записи.");
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<OrderDto> GetOrders(int id) => Session.Execute<IReadOnlyList<OrderDto>>(() =>
    {
        _ = Require(Session.Cars, id);
        return [.. Session.Orders.Find(order => order.Car.Id == id).Select(entity => entity.ToDto())];
    });
    /// <inheritdoc />
    public ClientDto GetOwner(int id) => Session.Execute(() => Require(Session.Cars, id).Owner.ToDto());
}
