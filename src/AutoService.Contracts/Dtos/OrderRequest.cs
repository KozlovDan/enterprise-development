using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.Dtos;

/// <summary>Заказ: данные для создания или полного обновления.</summary>
public class OrderRequest : IValidatableObject
{
    /// <summary>Идентификатор автомобиля.</summary>
    [Required, Range(1, int.MaxValue)]
    public int? CarId { get; init; }

    /// <summary>Идентификатор клиента, оформляющего заказ.</summary>
    [Required, Range(1, int.MaxValue)]
    public int? ClientId { get; init; }

    /// <summary>Идентификатор ответственного механика.</summary>
    [Required, Range(1, int.MaxValue)]
    public int? MechanicId { get; init; }

    /// <summary>Непустой список уникальных идентификаторов работ.</summary>
    [Required, MinLength(1)]
    public IReadOnlyList<int> WorkTypeIds { get; init; } = [];

    /// <summary>Дата и время приёма с часовым поясом.</summary>
    [Required]
    public DateTimeOffset? AcceptedAt { get; init; }

    /// <summary>Дата выдачи; null, пока заказ не завершён.</summary>
    public DateTimeOffset? ReturnedAt { get; init; }

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AcceptedAt is { Year: < 2000 })
        {
            yield return new("Дата приёма должна быть не раньше 2000 года.", [nameof(AcceptedAt)]);
        }
        if (ReturnedAt < AcceptedAt)
        {
            yield return new("Дата выдачи не может быть раньше даты приёма.", [nameof(ReturnedAt)]);
        }
        if (WorkTypeIds is not null &&
            (WorkTypeIds.Any(id => id <= 0) || WorkTypeIds.Distinct().Count() != WorkTypeIds.Count))
        {
            yield return new("Идентификаторы работ должны быть положительными и уникальными.", [nameof(WorkTypeIds)]);
        }
    }
}
