using AutoService.Domain.Interfaces;
namespace AutoService.Domain.Entities;

/// <summary>Автомобиль, принадлежащий клиенту автосервиса.</summary>
public class Car : IEntity
{
    /// <summary>Идентификатор автомобиля.</summary>
    public required int Id { get; set; }

    /// <summary>Государственный регистрационный номер, включая регион.</summary>
    public required string RegistrationNumber { get; set; }

    /// <summary>Марка автомобиля.</summary>
    public required string Brand { get; set; }

    /// <summary>Модель автомобиля.</summary>
    public required string Model { get; set; }

    /// <summary>Год выпуска автомобиля.</summary>
    public required int ManufactureYear { get; set; }

    /// <summary>Владелец автомобиля.</summary>
    public required Client Owner { get; set; }
}
