namespace AutoService.Domain.Entities;

/// <summary>Заказ на обслуживание автомобиля.</summary>
public class Order
{
    /// <summary>Идентификатор заказа.</summary>
    public required int Id { get; init; }

    /// <summary>Обслуживаемый автомобиль.</summary>
    public required Car Car { get; init; }

    /// <summary>Клиент, оформивший заказ.</summary>
    public required Client Client { get; init; }

    /// <summary>Механик, ответственный за заказ.</summary>
    public required Mechanic Mechanic { get; init; }

    /// <summary>Виды работ в заказе. Каждый вид включается не более одного раза.</summary>
    public required IReadOnlyList<WorkType> WorkTypes { get; init; }

    /// <summary>Дата и время приёма автомобиля.</summary>
    public required DateTimeOffset AcceptedAt { get; init; }

    /// <summary>Дата и время выдачи; null, если автомобиль ещё в сервисе.</summary>
    public DateTimeOffset? ReturnedAt { get; init; }
}
