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
    protected override Mechanic Build(MechanicRequest request, int id)
    {
        var specialization = (Domain.Enums.Specialization)request.Specialization!.Value;
        var passport = request.PassportNumber.Trim();
        if (Session.Mechanics.Exists(mechanic => mechanic.Id != id && mechanic.PassportNumber == passport))
        {
            throw new EntityConflictException("Механик с таким паспортом уже существует.");
        }
        if (Session.Orders.Exists(order => order.Mechanic.Id == id &&
            order.WorkTypes.Any(work => work.Category != specialization)))
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
    protected override void CheckDelete(Mechanic entity)
    {
        if (Session.Orders.Exists(order => order.Mechanic.Id == entity.Id))
        {
            throw new EntityConflictException("Запись используется. Сначала удалите связанные записи.");
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<OrderDto> GetOrders(int id) => Session.Execute<IReadOnlyList<OrderDto>>(() =>
    {
        _ = Require(Session.Mechanics, id);
        return [.. Session.Orders.Find(order => order.Mechanic.Id == id).Select(entity => entity.ToDto())];
    });
    /// <inheritdoc />
    public IReadOnlyList<ClientDto> GetClients(int id) => Session.Execute<IReadOnlyList<ClientDto>>(() =>
    {
        _ = Require(Session.Mechanics, id);
        return [.. Session.Orders.GetClientsByMechanic(id).Select(client => client.ToDto())];
    });
}
