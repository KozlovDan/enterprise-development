using AutoService.Domain.Interfaces;
using AutoService.Domain.Enums;

namespace AutoService.Domain.Entities;

/// <summary>Вид работ в прейскуранте автосервиса.</summary>
public class WorkType : IEntity
{
    /// <summary>Идентификатор вида работ.</summary>
    public required int Id { get; set; }

    /// <summary>Название работы.</summary>
    public required string Name { get; set; }

    /// <summary>Категория работы, определяющая необходимую специализацию.</summary>
    public required Specialization Category { get; set; }

    /// <summary>Стоимость одного выполнения работы в рублях.</summary>
    public required decimal Cost { get; set; }

    /// <summary>Нормативная продолжительность работы.</summary>
    public required TimeSpan Duration { get; set; }
}
