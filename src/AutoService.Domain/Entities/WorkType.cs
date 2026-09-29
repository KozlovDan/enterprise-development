using AutoService.Domain.Enums;

namespace AutoService.Domain.Entities;

/// <summary>Вид работ в прейскуранте автосервиса.</summary>
public class WorkType
{
    /// <summary>Идентификатор вида работ.</summary>
    public required int Id { get; init; }

    /// <summary>Название работы.</summary>
    public required string Name { get; init; }

    /// <summary>Категория работы, определяющая необходимую специализацию.</summary>
    public required Specialization Category { get; init; }

    /// <summary>Стоимость одного выполнения работы в рублях.</summary>
    public required decimal Cost { get; init; }

    /// <summary>Нормативная продолжительность работы.</summary>
    public required TimeSpan Duration { get; init; }
}
