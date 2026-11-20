namespace AutoService.Contracts.Dtos;

/// <summary>Данные клиента в ответах API.</summary>
public class ClientDto
{
    /// <summary>Идентификатор клиента.</summary>
    public required int Id { get; init; }

    /// <summary>Фамилия, имя и отчество.</summary>
    public required string FullName { get; init; }

    /// <summary>Контактный телефон с кодом страны.</summary>
    public required string Phone { get; init; }
}
