namespace AutoService.Contracts.Dtos;

/// <summary>Повторные обращения клиента.</summary>
/// <param name="ClientId">Идентификатор клиента.</param>
/// <param name="Count">Количество повторных обращений.</param>
public record RepeatVisitDto(int ClientId, int Count);
