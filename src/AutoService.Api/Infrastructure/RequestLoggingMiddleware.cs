using System.Diagnostics;

namespace AutoService.Api.Infrastructure;

/// <summary>Единообразно логирует начало и завершение HTTP-запросов.</summary>
/// <param name="next">Следующий обработчик.</param>
/// <param name="logger">Журнал запросов.</param>
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    /// <summary>Записывает маршрут, код ответа и время обработки.</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var start = Stopwatch.GetTimestamp();
        logger.LogInformation("Запрос {Method} {Path}", context.Request.Method, context.Request.Path);
        try
        {
            await next(context);
        }
        finally
        {
            logger.LogInformation("Ответ {Method} {Path}: {Status}, {ElapsedMs:F1} мс",
                context.Request.Method, context.Request.Path, context.Response.StatusCode,
                Stopwatch.GetElapsedTime(start).TotalMilliseconds);
        }
    }
}
