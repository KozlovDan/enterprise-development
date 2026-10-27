using System.ComponentModel.DataAnnotations;
using AutoService.Contracts.Enums;

namespace AutoService.Contracts.Dtos;

/// <summary>Механик: данные для создания или полного обновления.</summary>
public class MechanicRequest
{
    /// <summary>Фамилия, имя и отчество.</summary>
    [Required, StringLength(200)]
    public string FullName { get; init; } = string.Empty;

    /// <summary>Серия и номер паспорта: 0000 000000.</summary>
    [Required, RegularExpression(@"\d{4} \d{6}")]
    public string PassportNumber { get; init; } = string.Empty;

    /// <summary>Специализация механика.</summary>
    [Required, EnumDataType(typeof(Specialization))]
    public Specialization? Specialization { get; init; }

    /// <summary>Стаж работы в полных годах.</summary>
    [Required, Range(0, 80)]
    public int? ExperienceYears { get; init; }
}
