namespace AutoService.Contracts.Dtos;

/// <summary>Стоимость работ заказа.</summary>
/// <param name="OrderId">Идентификатор заказа.</param>
/// <param name="TotalCost">Сумма стоимости работ в рублях.</param>
public record OrderCostDto(int OrderId, decimal TotalCost);
