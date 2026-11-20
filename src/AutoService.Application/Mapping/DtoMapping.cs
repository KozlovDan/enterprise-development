using AutoService.Contracts.Dtos;
using AutoService.Domain.Entities;

namespace AutoService.Application.Mapping;

/// <summary>Преобразования сущностей в контракты API.</summary>
public static class DtoMapping
{
    /// <summary>Возвращает DTO автомобиля с идентификатором владельца.</summary>
    public static CarDto ToDto(this Car entity) => new()
    {
        Id = entity.Id,
        RegistrationNumber = entity.RegistrationNumber,
        Brand = entity.Brand,
        Model = entity.Model,
        ManufactureYear = entity.ManufactureYear,
        OwnerId = entity.OwnerId
    };

    /// <summary>Возвращает DTO заказа с идентификаторами автомобиля, клиента, механика и работ.</summary>
    public static OrderDto ToDto(this Order entity) => new()
    {
        Id = entity.Id,
        CarId = entity.CarId,
        ClientId = entity.ClientId,
        MechanicId = entity.MechanicId,
        WorkTypeIds = [.. entity.WorkTypes.Select(work => work.Id)],
        AcceptedAt = entity.AcceptedAt,
        ReturnedAt = entity.ReturnedAt
    };

    /// <summary>Возвращает DTO вида работ с продолжительностью в минутах.</summary>
    public static WorkTypeDto ToDto(this WorkType entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Category = (Contracts.Enums.Specialization)entity.Category,
        Cost = entity.Cost,
        DurationMinutes = (int)entity.Duration.TotalMinutes
    };

    /// <summary>Возвращает DTO механика с паспортом, специализацией и стажем.</summary>
    public static MechanicDto ToDto(this Mechanic entity) => new()
    {
        Id = entity.Id,
        FullName = entity.FullName,
        PassportNumber = entity.PassportNumber,
        Specialization = (Contracts.Enums.Specialization)entity.Specialization,
        ExperienceYears = entity.ExperienceYears
    };

    /// <summary>Возвращает DTO клиента с ФИО и телефоном.</summary>
    public static ClientDto ToDto(this Client entity) => new()
    {
        Id = entity.Id,
        FullName = entity.FullName,
        Phone = entity.Phone
    };
}
