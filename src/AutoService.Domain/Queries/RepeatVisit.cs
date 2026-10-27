namespace AutoService.Domain.Queries;

/// <summary>Количество повторных обращений клиента.</summary>
/// <param name="ClientId">Идентификатор клиента.</param>
/// <param name="Count">Количество повторных обращений.</param>
public record RepeatVisit(int ClientId, int Count);
