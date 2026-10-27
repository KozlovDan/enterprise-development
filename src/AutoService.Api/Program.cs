using System.Text.Json.Serialization;
using AutoService.Api.Infrastructure;
using AutoService.Application.Interfaces;
using AutoService.Application.Services;
using AutoService.Domain.Interfaces;
using AutoService.Infrastructure.InMemory;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));
builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    document.Info.Title = "Автосервис";
    document.Info.Description = "Лабораторная работа №2. Козлов Данила, группа 6412. Вариант 46.";
    return Task.CompletedTask;
}));
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddSingleton<IAutoServiceSession, AutoServiceSession>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IMechanicService, MechanicService>();
builder.Services.AddScoped<IWorkTypeService, WorkTypeService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandler();
app.UseStatusCodePages();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}
app.MapControllers();
app.Run();

/// <summary>Точка входа для интеграционных тестов API.</summary>
public partial class Program;
