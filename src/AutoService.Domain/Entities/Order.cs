using AutoService.Domain.Interfaces;
namespace AutoService.Domain.Entities;

/// <summary>Заказ на обслуживание автомобиля.</summary>
public class Order : IEntity
{
    /// <summary>Идентификатор заказа.</summary>
    public required int Id { get; set; }

    /// <summary>Обслуживаемый автомобиль.</summary>
    public required Car Car { get; set; }

    /// <summary>Клиент, оформивший заказ.</summary>
    public required Client Client { get; set; }

    /// <summary>Механик, ответственный за заказ.</summary>
    public required Mechanic Mechanic { get; set; }

    /// <summary>Виды работ в заказе. Каждый вид включается не более одного раза.</summary>
    public required IReadOnlyList<WorkType> WorkTypes { get; set; }

    /// <summary>Дата и время приёма автомобиля.</summary>
    public required DateTimeOffset AcceptedAt { get; set; }

    /// <summary>Дата и время выдачи; null, если автомобиль ещё в сервисе.</summary>
    public DateTimeOffset? ReturnedAt { get; set; }
}
