using AutoService.Application.Interfaces;
using AutoService.Application.Mapping;
using AutoService.Contracts.Dtos;
using AutoService.Domain.Entities;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;

namespace AutoService.Application.Services;

/// <summary>Работа с клиентами и проверка связанных данных.</summary>
/// <param name="session">Сеанс хранилища.</param>
public class ClientService(IAutoServiceSession session)
    : CrudService<Client, ClientRequest, ClientDto>(session, session.Clients), IClientService
{
    /// <inheritdoc />
    protected override Client Build(ClientRequest request, int id)
    {
        return new()
        {
            Id = id,
            FullName = request.FullName.Trim(),
            Phone = request.Phone.Trim()
        };
    }

    /// <inheritdoc />
    protected override void Copy(Client source, Client target)
    {
        target.FullName = source.FullName;
        target.Phone = source.Phone;
    }

    /// <inheritdoc />
    protected override ClientDto ToDto(Client entity) => entity.ToDto();

    /// <inheritdoc />
    protected override void CheckDelete(Client entity)
    {
        if (Session.Cars.Exists(car => car.Owner.Id == entity.Id) ||
            Session.Orders.Exists(order => order.Client.Id == entity.Id))
        {
            throw new EntityConflictException("Запись используется. Сначала удалите связанные записи.");
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<CarDto> GetCars(int id) => Session.Execute<IReadOnlyList<CarDto>>(() =>
    {
        _ = Require(Session.Clients, id);
        return [.. Session.Cars.Find(car => car.Owner.Id == id).Select(entity => entity.ToDto())];
    });

    /// <inheritdoc />
    public IReadOnlyList<OrderDto> GetOrders(int id) => Session.Execute<IReadOnlyList<OrderDto>>(() =>
    {
        _ = Require(Session.Clients, id);
        return [.. Session.Orders.Find(order => order.Client.Id == id).Select(entity => entity.ToDto())];
    });
}
