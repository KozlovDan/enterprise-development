namespace AutoService.Domain.Queries;

/// <summary>Частота выполнения вида работ.</summary>
/// <param name="WorkTypeId">Идентификатор вида работ.</param>
/// <param name="Count">Количество заказов с этим видом работ.</param>
public record WorkFrequency(int WorkTypeId, int Count);
