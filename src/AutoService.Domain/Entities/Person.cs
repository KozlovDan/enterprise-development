using AutoService.Domain.Interfaces;
namespace AutoService.Domain.Entities;

/// <summary>
/// Общие сведения о клиенте или механике автосервиса.
/// </summary>
public abstract class Person : IEntity
{
    /// <summary>Идентификатор человека в соответствующем справочнике.</summary>
    public required int Id { get; set; }

    /// <summary>Фамилия, имя и отчество (при наличии) в указанном порядке.</summary>
    public required string FullName { get; set; }
}
