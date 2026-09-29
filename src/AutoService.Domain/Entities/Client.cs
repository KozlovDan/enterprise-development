namespace AutoService.Domain.Entities;

/// <summary>
/// Клиент автосервиса. Одному клиенту могут принадлежать несколько автомобилей.
/// </summary>
public class Client : Person
{
    /// <summary>Контактный телефон с кодом страны.</summary>
    public required string Phone { get; init; }
}
