using AutoService.Application.Interfaces;
using AutoService.Application.Mapping;
using AutoService.Contracts.Dtos;
using AutoService.Domain.Entities;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;

namespace AutoService.Application.Services;

/// <summary>Работа с механиками и проверка связанных данных.</summary>
/// <param name="session">Сеанс хранилища.</param>
public class MechanicService(IAutoServiceSession session)
    : CrudService<Mechanic, MechanicRequest, MechanicDto>(session, session.Mechanics), IMechanicService
{
    /// <inheritdoc />
    protected override async Task<Mechanic> BuildAsync(MechanicRequest request, int id)
    {
        var specialization = (Domain.Enums.Specialization)request.Specialization!.Value;
        var passport = request.PassportNumber.Trim();
        if ((await Session.Mechanics.ExistsAsync(mechanic => mechanic.Id != id && mechanic.PassportNumber == passport)))
        {
            throw new EntityConflictException("Механик с таким паспортом уже существует.");
        }
        if ((await Session.Orders.ExistsAsync(order => order.MechanicId == id &&
            order.WorkTypes.Any(work => work.Category != specialization))))
        {
            throw new EntityConflictException("Новая специализация не соответствует работам в заказах механика.");
        }
        return new()
        {
            Id = id,
            FullName = request.FullName.Trim(),
            PassportNumber = passport,
            Specialization = specialization,
            ExperienceYears = request.ExperienceYears!.Value
        };
    }

    /// <inheritdoc />
    protected override void Copy(Mechanic source, Mechanic target)
    {
        target.FullName = source.FullName;
        target.PassportNumber = source.PassportNumber;
        target.Specialization = source.Specialization;
        target.ExperienceYears = source.ExperienceYears;
    }

    /// <inheritdoc />
    protected override MechanicDto ToDto(Mechanic entity) => entity.ToDto();

    /// <inheritdoc />
    protected override async Task CheckDeleteAsync(Mechanic entity)
    {
        if ((await Session.Orders.ExistsAsync(order => order.MechanicId == entity.Id)))
        {
            throw new EntityConflictException("Запись используется. Сначала удалите связанные записи.");
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<OrderDto>> GetOrdersAsync(int id) => Session.ExecuteReadAsync<IReadOnlyList<OrderDto>>(async () =>
    {
        _ = await RequireAsync(Session.Mechanics, id);
        return [.. (await Session.Orders.FindAsync(order => order.MechanicId == id)).Select(entity => entity.ToDto())];
    });
    /// <inheritdoc />
    public Task<IReadOnlyList<ClientDto>> GetClientsAsync(int id) => Session.ExecuteReadAsync<IReadOnlyList<ClientDto>>(async () =>
    {
        _ = await RequireAsync(Session.Mechanics, id);
        return [.. (await Session.Orders.GetClientsByMechanicAsync(id)).Select(client => client.ToDto())];
    });
}
