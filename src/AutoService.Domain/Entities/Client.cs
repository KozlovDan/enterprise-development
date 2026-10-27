namespace AutoService.Domain.Entities;

/// <summary>
/// Клиент автосервиса.
/// </summary>
public class Client : Person
{
    /// <summary>Контактный телефон с кодом страны.</summary>
    public required string Phone { get; set; }
}
