namespace AutoService.Contracts.Dtos;

/// <summary>Заказ: представление записи API.</summary>
public class OrderDto
{
    /// <summary>Идентификатор записи.</summary>
    public required int Id { get; init; }

    /// <summary>Идентификатор автомобиля.</summary>
    public required int CarId { get; init; }

    /// <summary>Идентификатор клиента, оформляющего заказ.</summary>
    public required int ClientId { get; init; }

    /// <summary>Идентификатор ответственного механика.</summary>
    public required int MechanicId { get; init; }

    /// <summary>Непустой список уникальных идентификаторов работ.</summary>
    public required IReadOnlyList<int> WorkTypeIds { get; init; }

    /// <summary>Дата и время приёма с часовым поясом.</summary>
    public required DateTimeOffset AcceptedAt { get; init; }

    /// <summary>Дата выдачи; null, пока заказ не завершён.</summary>
    public required DateTimeOffset? ReturnedAt { get; init; }
}
