namespace AutoService.Contracts.Dtos;

/// <summary>Популярность вида работ.</summary>
/// <param name="WorkTypeId">Идентификатор вида работ.</param>
/// <param name="Count">Количество заказов с данным видом работ.</param>
public record WorkFrequencyDto(int WorkTypeId, int Count);
