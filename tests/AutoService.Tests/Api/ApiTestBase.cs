using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoService.Tests.Api;

/// <summary>Каждый тест получает отдельный HTTP-сервер с исходным датасетом.</summary>
public abstract class ApiTestBase : IDisposable
{
    /// <summary>Параметры чтения строковых перечислений из API.</summary>
    protected static JsonSerializerOptions JsonOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureLogging(logging => logging.ClearProviders());
        });

    /// <summary>Клиент тестового сервера.</summary>
    protected HttpClient Client { get; }

    /// <summary>Запускает изолированный экземпляр приложения.</summary>
    protected ApiTestBase()
    {
        Client = _factory.CreateClient();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Client.Dispose();
        _factory.Dispose();
        GC.SuppressFinalize(this);
    }
}
