using AutoService.Contracts.Enums;
namespace AutoService.Contracts.Dtos;

/// <summary>Вид работ: представление записи API.</summary>
public class WorkTypeDto
{
    /// <summary>Идентификатор записи.</summary>
    public required int Id { get; init; }

    /// <summary>Название работы.</summary>
    public required string Name { get; init; }

    /// <summary>Необходимая специализация.</summary>
    public required Specialization Category { get; init; }

    /// <summary>Стоимость в рублях.</summary>
    public required decimal Cost { get; init; }

    /// <summary>Нормативная продолжительность в минутах.</summary>
    public required int DurationMinutes { get; init; }
}
