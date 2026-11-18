using System.Text.Json.Serialization;
using AutoService.Api.Infrastructure;
using AutoService.Application.Interfaces;
using AutoService.Application.Services;
using AutoService.Domain.Interfaces;
using AutoService.Infrastructure.MySql;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));
builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    document.Info.Title = "Автосервис";
    document.Info.Description = "Лабораторная работа №3. Козлов Данила, группа 6412. Вариант 46.";
    return Task.CompletedTask;
}));
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddDbContext<AutoServiceDbContext>((services, options) =>
{
    var connection = services.GetRequiredService<IConfiguration>().GetConnectionString("autoservice")
        ?? throw new InvalidOperationException("Не задана строка подключения autoservice. Запустите приложение через AppHost.");
    options.UseMySQL(connection, mysql => mysql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(1), [1205, 1213]));
});
builder.Services.AddScoped<IAutoServiceSession, EfAutoServiceSession>();
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
app.MapGet("/health", () => Results.Ok()).ExcludeFromDescription();
app.Run();
