using System.ComponentModel.DataAnnotations;
using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Сводные отчёты по заказам.</summary>
/// <param name="service">Сервис аналитики.</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(IAnalyticsService service) : ControllerBase
{
    /// <summary>Повторные обращения за интервал [reportDate минус месяц, reportDate).</summary>
    /// <remarks>Первое обращение исключается из всей истории клиента. Клиенты без повторных визитов не включаются.</remarks>
    /// <param name="reportDate">Дата отчёта с часовым поясом, например 2026-09-27T12:00:00Z.</param>
    [HttpGet("repeat-visits")]
    [ProducesResponseType<IReadOnlyList<RepeatVisitDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public ActionResult<IReadOnlyList<RepeatVisitDto>> GetRepeatVisits([FromQuery, Required] DateTimeOffset? reportDate) =>
        Ok(service.GetRepeatVisits(reportDate!.Value));

    /// <summary>Топ-5 наиболее частых видов работ за всю историю.</summary>
    /// <remarks>При одинаковом количестве заказов первым идёт вид работ с меньшим идентификатором.</remarks>
    [HttpGet("most-frequent-work-types")]
    [ProducesResponseType<IReadOnlyList<WorkFrequencyDto>>(StatusCodes.Status200OK, "application/json")]
    public ActionResult<IReadOnlyList<WorkFrequencyDto>> GetMostFrequentWorkTypes() =>
        Ok(service.GetMostFrequentWorkTypes());
}
