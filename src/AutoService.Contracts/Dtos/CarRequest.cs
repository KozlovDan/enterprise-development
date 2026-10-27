using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.Dtos;

/// <summary>Автомобиль: данные для создания или полного обновления.</summary>
public class CarRequest
{
    /// <summary>Государственный регистрационный номер.</summary>
    [Required, StringLength(15)]
    public string RegistrationNumber { get; init; } = string.Empty;

    /// <summary>Марка автомобиля.</summary>
    [Required, StringLength(100)]
    public string Brand { get; init; } = string.Empty;

    /// <summary>Модель автомобиля.</summary>
    [Required, StringLength(100)]
    public string Model { get; init; } = string.Empty;

    /// <summary>Год выпуска, не позднее текущего года.</summary>
    [Required, Range(1886, 9999)]
    public int? ManufactureYear { get; init; }

    /// <summary>Идентификатор владельца.</summary>
    [Required, Range(1, int.MaxValue)]
    public int? OwnerId { get; init; }
}
