using AutoService.Application.Interfaces;
using AutoService.Application.Mapping;
using AutoService.Contracts.Dtos;
using AutoService.Domain.Entities;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;

namespace AutoService.Application.Services;

/// <summary>Работа с видами работ и проверка связанных данных.</summary>
/// <param name="session">Сеанс хранилища.</param>
public class WorkTypeService(IAutoServiceSession session)
    : CrudService<WorkType, WorkTypeRequest, WorkTypeDto>(session, session.WorkTypes), IWorkTypeService
{
    /// <inheritdoc />
    protected override async Task<WorkType> BuildAsync(WorkTypeRequest request, int id)
    {
        var category = (Domain.Enums.Specialization)request.Category!.Value;
        if ((await Session.Orders.ExistsAsync(order => order.WorkTypes.Any(work => work.Id == id) &&
            order.Mechanic.Specialization != category)))
        {
            throw new EntityConflictException("Новая категория не соответствует механикам в существующих заказах.");
        }
        return new()
        {
            Id = id,
            Name = request.Name.Trim(),
            Category = category,
            Cost = request.Cost!.Value,
            Duration = TimeSpan.FromMinutes(request.DurationMinutes!.Value)
        };
    }

    /// <inheritdoc />
    protected override void Copy(WorkType source, WorkType target)
    {
        target.Name = source.Name;
        target.Category = source.Category;
        target.Cost = source.Cost;
        target.Duration = source.Duration;
    }

    /// <inheritdoc />
    protected override WorkTypeDto ToDto(WorkType entity) => entity.ToDto();

    /// <inheritdoc />
    protected override async Task CheckDeleteAsync(WorkType entity)
    {
        if ((await Session.Orders.ExistsAsync(order => order.WorkTypes.Any(work => work.Id == entity.Id))))
        {
            throw new EntityConflictException("Запись используется. Сначала удалите связанные записи.");
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<OrderDto>> GetOrdersAsync(int id) => Session.ExecuteReadAsync<IReadOnlyList<OrderDto>>(async () =>
    {
        _ = await RequireAsync(Session.WorkTypes, id);
        return [.. (await Session.Orders.FindAsync(order => order.WorkTypes.Any(work => work.Id == id))).Select(entity => entity.ToDto())];
    });
    /// <inheritdoc />
    public Task<IReadOnlyList<MechanicDto>> GetMechanicsAsync(int id) => Session.ExecuteReadAsync<IReadOnlyList<MechanicDto>>(async () =>
    {
        var work = await RequireAsync(Session.WorkTypes, id);
        return [.. (await Session.Mechanics.FindAsync(mechanic => mechanic.Specialization == work.Category))
            .Select(mechanic => mechanic.ToDto())];
    });
}
