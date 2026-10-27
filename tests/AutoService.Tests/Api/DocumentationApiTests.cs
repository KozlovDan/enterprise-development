using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using AutoService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.Tests.Api;

/// <summary>Контракты и описания должны быть видны в документации API.</summary>
public class DocumentationApiTests : ApiTestBase
{
    /// <summary>Неожиданная ошибка не раскрывает клиенту внутренние сведения.</summary>
    [Fact]
    public async Task UnexpectedFailure_ReturnsSafeProblemDetails()
    {
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                services.AddScoped<IClientService>(_ => throw new InvalidOperationException("internal-secret"))));
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/api/clients", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.DoesNotContain("internal-secret", body);
        Assert.DoesNotContain("stackTrace", body, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Спецификация содержит типизированные ответы, комментарии методов и свойств DTO.</summary>
    [Fact]
    public async Task OpenApi_ContainsDocumentedContracts()
    {
        var document = await Client.GetFromJsonAsync<JsonElement>("/openapi/v1.json", cancellationToken: TestContext.Current.CancellationToken);
        var paths = document.GetProperty("paths");
        var clients = paths.GetProperty("/api/clients");
        Assert.False(string.IsNullOrWhiteSpace(clients.GetProperty("get").GetProperty("summary").GetString()));
        Assert.True(clients.GetProperty("post").GetProperty("responses").TryGetProperty("201", out _));
        Assert.True(clients.GetProperty("get").GetProperty("responses").GetProperty("200")
            .GetProperty("content").TryGetProperty("application/json", out _));
        Assert.True(clients.GetProperty("post").GetProperty("responses").GetProperty("400")
            .GetProperty("content").TryGetProperty("application/problem+json", out _));
        var schema = document.GetProperty("components").GetProperty("schemas");
        var dto = schema.GetProperty("ClientDto");
        Assert.False(string.IsNullOrWhiteSpace(dto.GetProperty("properties").GetProperty("phone")
            .GetProperty("description").GetString()));
    }

    /// <summary>Scalar доступен в режиме разработки.</summary>
    [Fact]
    public async Task Scalar_IsAvailableInDevelopment()
    {
        using var response = await Client.GetAsync("/scalar", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("scalar", await response.Content.ReadAsStringAsync(cancellationToken: TestContext.Current.CancellationToken), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Неизвестные маршруты также возвращают ProblemDetails.</summary>
    [Fact]
    public async Task MissingRoute_ReturnsProblemDetails()
    {
        using var response = await Client.GetAsync("/api/not-a-resource", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    /// <summary>В production документация не публикуется, а API работает.</summary>
    [Fact]
    public async Task Production_ExposesOnlyApi()
    {
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
        using var client = factory.CreateClient();
        using var openApi = await client.GetAsync("/openapi/v1.json", cancellationToken: TestContext.Current.CancellationToken);
        using var scalar = await client.GetAsync("/scalar", cancellationToken: TestContext.Current.CancellationToken);
        using var api = await client.GetAsync("/api/clients", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, openApi.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, scalar.StatusCode);
        Assert.Equal(HttpStatusCode.OK, api.StatusCode);
    }
}
