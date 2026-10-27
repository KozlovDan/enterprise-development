using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.Dtos;

/// <summary>Клиент: данные для создания или полного обновления.</summary>
public class ClientRequest
{
    /// <summary>Фамилия, имя и отчество.</summary>
    [Required, StringLength(200)]
    public string FullName { get; init; } = string.Empty;

    /// <summary>Контактный телефон с кодом страны.</summary>
    [Required, Phone, StringLength(30)]
    public string Phone { get; init; } = string.Empty;
}
