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
    protected override async Task<Car> BuildAsync(CarRequest request, int id)
    {
        var owner = await ReferenceAsync(Session.Clients, request.OwnerId!.Value, "Клиент");
        var number = request.RegistrationNumber.Trim().ToUpperInvariant();
        if (request.ManufactureYear > clock.GetUtcNow().Year)
        {
            throw new DomainValidationException("Год выпуска не может быть позже текущего года.");
        }
        if ((await Session.Cars.ExistsAsync(car => car.Id != id && car.RegistrationNumber == number)))
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
            OwnerId = owner.Id,
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
        target.OwnerId = source.OwnerId;
        target.Owner = source.Owner;
    }

    /// <inheritdoc />
    protected override CarDto ToDto(Car entity) => entity.ToDto();

    /// <inheritdoc />
    protected override async Task CheckDeleteAsync(Car entity)
    {
        if ((await Session.Orders.ExistsAsync(order => order.CarId == entity.Id)))
        {
            throw new EntityConflictException("Запись используется. Сначала удалите связанные записи.");
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<OrderDto>> GetOrdersAsync(int id) => Session.ExecuteAsync<IReadOnlyList<OrderDto>>(async () =>
    {
        _ = await RequireAsync(Session.Cars, id);
        return [.. (await Session.Orders.FindAsync(order => order.CarId == id)).Select(entity => entity.ToDto())];
    });
    /// <inheritdoc />
    public Task<ClientDto> GetOwnerAsync(int id) => Session.ExecuteAsync(async () =>
    {
        var car = await RequireAsync(Session.Cars, id);
        return (await RequireAsync(Session.Clients, car.OwnerId)).ToDto();
    });
}
