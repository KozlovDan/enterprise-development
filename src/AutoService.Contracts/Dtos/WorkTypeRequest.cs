using System.ComponentModel.DataAnnotations;
using AutoService.Contracts.Enums;

namespace AutoService.Contracts.Dtos;

/// <summary>Вид работ: данные для создания или полного обновления.</summary>
public class WorkTypeRequest
{
    /// <summary>Название работы.</summary>
    [Required, StringLength(200)]
    public string Name { get; init; } = string.Empty;

    /// <summary>Необходимая специализация.</summary>
    [Required, EnumDataType(typeof(Specialization))]
    public Specialization? Category { get; init; }

    /// <summary>Стоимость в рублях.</summary>
    [Required, Range(typeof(decimal), "0.01", "1000000000", ParseLimitsInInvariantCulture = true)]
    public decimal? Cost { get; init; }

    /// <summary>Нормативная продолжительность в минутах.</summary>
    [Required, Range(1, 525600)]
    public int? DurationMinutes { get; init; }
}
