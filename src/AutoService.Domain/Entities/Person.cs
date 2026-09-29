namespace AutoService.Domain.Entities;

/// <summary>
/// Общие сведения о клиенте или механике автосервиса.
/// </summary>
public abstract class Person
{
    /// <summary>Идентификатор человека в соответствующем справочнике.</summary>
    public required int Id { get; init; }

    /// <summary>Фамилия, имя и отчество (при наличии) в указанном порядке.</summary>
    public required string FullName { get; init; }
}
