using AutoService.Domain.Enums;

namespace AutoService.Domain.Entities;

/// <summary>Механик, выполняющий работы своей специализации.</summary>
public class Mechanic : Person
{
    /// <summary>Серия и номер паспорта. Строка сохраняет ведущие нули.</summary>
    public required string PassportNumber { get; init; }

    /// <summary>Специализация механика.</summary>
    public required Specialization Specialization { get; init; }

    /// <summary>Стаж работы в полных годах.</summary>
    public required int ExperienceYears { get; init; }
}
