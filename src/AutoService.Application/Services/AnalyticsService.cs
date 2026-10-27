using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using AutoService.Domain.Exceptions;
using AutoService.Domain.Interfaces;

namespace AutoService.Application.Services;

/// <summary>Преобразует результаты аналитики хранилища в контракты API.</summary>
/// <param name="session">Сеанс хранилища.</param>
public class AnalyticsService(IAutoServiceSession session) : IAnalyticsService
{
    /// <inheritdoc />
    public IReadOnlyList<RepeatVisitDto> GetRepeatVisits(DateTimeOffset reportDate) =>
        session.Execute<IReadOnlyList<RepeatVisitDto>>(() =>
        {
            if (reportDate.Year < 2000)
            {
                throw new DomainValidationException("Дата отчёта должна быть не раньше 2000 года.");
            }
            return [.. session.Orders.GetRepeatVisits(reportDate)
                .Select(row => new RepeatVisitDto(row.ClientId, row.Count))];
        });

    /// <inheritdoc />
    public IReadOnlyList<WorkFrequencyDto> GetMostFrequentWorkTypes() =>
        session.Execute<IReadOnlyList<WorkFrequencyDto>>(() =>
            [.. session.Orders.GetMostFrequentWorkTypes()
                .Select(row => new WorkFrequencyDto(row.WorkTypeId, row.Count))]);
}
