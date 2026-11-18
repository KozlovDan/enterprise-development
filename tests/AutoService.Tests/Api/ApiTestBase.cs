using AutoService.Api.Controllers;
using AutoService.Tests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoService.Tests.Api;

/// <summary>HTTP-сервер и отдельная база MySQL с исходными данными для каждого теста.</summary>
public abstract class ApiTestBase : IAsyncLifetime
{
    /// <summary>Параметры чтения строковых перечислений из API.</summary>
    protected static JsonSerializerOptions JsonOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private WebApplicationFactory<ClientsController>? _factory;

    /// <summary>Изолированная тестовая база.</summary>
    protected MySqlTestDatabase Database { get; } = new();
    /// <summary>Клиент тестового сервера.</summary>
    protected HttpClient Client { get; private set; } = null!;

    /// <summary>Создаёт сервер, использующий ту же базу и новый контейнер зависимостей.</summary>
    protected WebApplicationFactory<ClientsController> CreateFactory(string environment = "Development") =>
        new WebApplicationFactory<ClientsController>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment(environment);
            builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(
                new Dictionary<string, string?> { ["ConnectionStrings:autoservice"] = Database.ConnectionString }));
            builder.ConfigureLogging(logging => logging.ClearProviders());
        });

    /// <inheritdoc />
    public async ValueTask InitializeAsync()
    {
        await Database.InitializeAsync();
        _factory = CreateFactory();
        Client = _factory.CreateClient();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        Client?.Dispose();
        if (_factory is not null)
        {
            await _factory.DisposeAsync();
        }
        await Database.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
