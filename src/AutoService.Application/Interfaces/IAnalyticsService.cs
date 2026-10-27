using AutoService.Contracts.Dtos;

namespace AutoService.Application.Interfaces;

/// <summary>Сводные аналитические запросы автосервиса.</summary>
public interface IAnalyticsService
{
    /// <summary>Количество повторных обращений за последний месяц.</summary>
    public IReadOnlyList<RepeatVisitDto> GetRepeatVisits(DateTimeOffset reportDate);
    /// <summary>Пять наиболее частых видов работ.</summary>
    public IReadOnlyList<WorkFrequencyDto> GetMostFrequentWorkTypes();
}
