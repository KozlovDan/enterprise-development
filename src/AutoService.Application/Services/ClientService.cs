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
    protected override Task<Client> BuildAsync(ClientRequest request, int id)
    {
        return Task.FromResult(new Client
        {
            Id = id,
            FullName = request.FullName.Trim(),
            Phone = request.Phone.Trim()
        });
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
    protected override async Task CheckDeleteAsync(Client entity)
    {
        if ((await Session.Cars.ExistsAsync(car => car.OwnerId == entity.Id)) ||
            (await Session.Orders.ExistsAsync(order => order.ClientId == entity.Id)))
        {
            throw new EntityConflictException("Запись используется. Сначала удалите связанные записи.");
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<CarDto>> GetCarsAsync(int id) => Session.ExecuteReadAsync<IReadOnlyList<CarDto>>(async () =>
    {
        _ = await RequireAsync(Session.Clients, id);
        return [.. (await Session.Cars.FindAsync(car => car.OwnerId == id)).Select(entity => entity.ToDto())];
    });

    /// <inheritdoc />
    public Task<IReadOnlyList<OrderDto>> GetOrdersAsync(int id) => Session.ExecuteReadAsync<IReadOnlyList<OrderDto>>(async () =>
    {
        _ = await RequireAsync(Session.Clients, id);
        return [.. (await Session.Orders.FindAsync(order => order.ClientId == id)).Select(entity => entity.ToDto())];
    });
}
