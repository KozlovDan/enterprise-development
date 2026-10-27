using AutoService.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Infrastructure;

/// <summary>Преобразует ожидаемые ошибки в ProblemDetails, скрывая внутренние ошибки сервера.</summary>
/// <param name="logger">Журнал ошибок.</param>
public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            EntityNotFoundException => (StatusCodes.Status404NotFound, "Запись не найдена"),
            DomainValidationException => (StatusCodes.Status400BadRequest, "Некорректные данные"),
            EntityConflictException => (StatusCodes.Status409Conflict, "Конфликт данных"),
            _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера")
        };
        if (status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Ошибка при обработке {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
        }
        else
        {
            logger.LogWarning(exception, "Запрос {Method} {Path} завершён с кодом {Status}",
                httpContext.Request.Method, httpContext.Request.Path, status);
        }
        ProblemDetails problem = status == StatusCodes.Status400BadRequest
            ? new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                ["request"] = [exception.Message]
            })
            : new();
        problem.Status = status;
        problem.Title = title;
        problem.Type = $"https://httpstatuses.com/{status}";
        problem.Detail = status == StatusCodes.Status500InternalServerError
            ? "Не удалось обработать запрос. Обратитесь к администратору с идентификатором запроса."
            : exception.Message;
        problem.Instance = httpContext.Request.Path;
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;
        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync((object)problem, options: null,
            contentType: "application/problem+json", cancellationToken: cancellationToken);
        return true;
    }
}
