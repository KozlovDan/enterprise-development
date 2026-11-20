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
    public Task<IReadOnlyList<RepeatVisitDto>> GetRepeatVisitsAsync(DateTimeOffset reportDate) =>
        session.ExecuteReadAsync<IReadOnlyList<RepeatVisitDto>>(async () =>
        {
            if (reportDate.Year < 2000)
            {
                throw new DomainValidationException("Дата отчёта должна быть не раньше 2000 года.");
            }
            return [.. (await session.Orders.GetRepeatVisitsAsync(reportDate))
                .Select(row => new RepeatVisitDto(row.ClientId, row.Count))];
        });

    /// <inheritdoc />
    public Task<IReadOnlyList<WorkFrequencyDto>> GetMostFrequentWorkTypesAsync() =>
        session.ExecuteReadAsync<IReadOnlyList<WorkFrequencyDto>>(async () =>
            [.. (await session.Orders.GetMostFrequentWorkTypesAsync())
                .Select(row => new WorkFrequencyDto(row.WorkTypeId, row.Count))]);
}
