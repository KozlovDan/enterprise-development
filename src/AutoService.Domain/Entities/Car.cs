namespace AutoService.Domain.Entities;

/// <summary>Автомобиль, принадлежащий клиенту автосервиса.</summary>
public class Car
{
    /// <summary>Идентификатор автомобиля.</summary>
    public required int Id { get; init; }

    /// <summary>Государственный регистрационный номер, включая регион.</summary>
    public required string RegistrationNumber { get; init; }

    /// <summary>Марка автомобиля.</summary>
    public required string Brand { get; init; }

    /// <summary>Модель автомобиля.</summary>
    public required string Model { get; init; }

    /// <summary>Год выпуска автомобиля.</summary>
    public required int ManufactureYear { get; init; }

    /// <summary>Владелец автомобиля.</summary>
    public required Client Owner { get; init; }
}
