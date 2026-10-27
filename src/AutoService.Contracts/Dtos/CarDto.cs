namespace AutoService.Contracts.Dtos;

/// <summary>Автомобиль: представление записи API.</summary>
public class CarDto
{
    /// <summary>Идентификатор записи.</summary>
    public required int Id { get; init; }

    /// <summary>Государственный регистрационный номер.</summary>
    public required string RegistrationNumber { get; init; }

    /// <summary>Марка автомобиля.</summary>
    public required string Brand { get; init; }

    /// <summary>Модель автомобиля.</summary>
    public required string Model { get; init; }

    /// <summary>Год выпуска, не позднее текущего года.</summary>
    public required int ManufactureYear { get; init; }

    /// <summary>Идентификатор владельца.</summary>
    public required int OwnerId { get; init; }
}
