using AutoService.Contracts.Dtos;
using AutoService.Domain.Entities;

namespace AutoService.Application.Mapping;

/// <summary>Преобразования сущностей в контракты API.</summary>
public static class DtoMapping
{
    /// <summary>Преобразует запись в независимый контракт.</summary>
    public static CarDto ToDto(this Car entity) => new()
    {
        Id = entity.Id,
        RegistrationNumber = entity.RegistrationNumber,
        Brand = entity.Brand,
        Model = entity.Model,
        ManufactureYear = entity.ManufactureYear,
        OwnerId = entity.Owner.Id
    };

    /// <summary>Преобразует запись в независимый контракт.</summary>
    public static OrderDto ToDto(this Order entity) => new()
    {
        Id = entity.Id,
        CarId = entity.Car.Id,
        ClientId = entity.Client.Id,
        MechanicId = entity.Mechanic.Id,
        WorkTypeIds = [.. entity.WorkTypes.Select(work => work.Id)],
        AcceptedAt = entity.AcceptedAt,
        ReturnedAt = entity.ReturnedAt
    };

    /// <summary>Преобразует запись в независимый контракт.</summary>
    public static WorkTypeDto ToDto(this WorkType entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Category = (Contracts.Enums.Specialization)entity.Category,
        Cost = entity.Cost,
        DurationMinutes = (int)entity.Duration.TotalMinutes
    };

    /// <summary>Преобразует запись в независимый контракт.</summary>
    public static MechanicDto ToDto(this Mechanic entity) => new()
    {
        Id = entity.Id,
        FullName = entity.FullName,
        PassportNumber = entity.PassportNumber,
        Specialization = (Contracts.Enums.Specialization)entity.Specialization,
        ExperienceYears = entity.ExperienceYears
    };

    /// <summary>Преобразует запись в независимый контракт.</summary>
    public static ClientDto ToDto(this Client entity) => new()
    {
        Id = entity.Id,
        FullName = entity.FullName,
        Phone = entity.Phone
    };
}
