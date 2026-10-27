using AutoService.Contracts.Enums;
namespace AutoService.Contracts.Dtos;

/// <summary>Механик: представление записи API.</summary>
public class MechanicDto
{
    /// <summary>Идентификатор записи.</summary>
    public required int Id { get; init; }

    /// <summary>Фамилия, имя и отчество.</summary>
    public required string FullName { get; init; }

    /// <summary>Серия и номер паспорта: 0000 000000.</summary>
    public required string PassportNumber { get; init; }

    /// <summary>Специализация механика.</summary>
    public required Specialization Specialization { get; init; }

    /// <summary>Стаж работы в полных годах.</summary>
    public required int ExperienceYears { get; init; }
}
