using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using Aspire.Hosting;
using Aspire.Hosting.Testing;

namespace AutoService.AspireTests;

/// <summary>Запуск полного приложения Aspire: контейнер MySQL, миграции и API.</summary>
public class AppHostTests
{
    /// <summary>API становится готовым после миграций и работает с заполненной базой.</summary>
    [Fact]
    public async Task AppHost_StartsDatabaseMigrationsAndApi()
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        timeout.CancelAfter(TimeSpan.FromMinutes(5));
        var token = timeout.Token;
        var password = Convert.ToHexString(RandomNumberGenerator.GetBytes(24));
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AutoService_AppHost>(
            ["Testing=true", $"Parameters:mysql-password={password}"], token);
        await using var app = await builder.BuildAsync(token);
        await app.StartAsync(token);
        await app.ResourceNotifications.WaitForResourceHealthyAsync("api", token);
        using var client = app.CreateHttpClient("api", "http");
        using var health = await client.GetAsync("/health", token);
        Assert.Equal(HttpStatusCode.OK, health.StatusCode);
        var orders = await client.GetFromJsonAsync<JsonElement>("/api/orders", token);
        Assert.Equal(28, orders.GetArrayLength());
        var cost = await client.GetFromJsonAsync<JsonElement>("/api/orders/1/total-cost", token);
        Assert.Equal(1600.75m, cost.GetProperty("totalCost").GetDecimal());
        using var created = await client.PostAsJsonAsync("/api/clients",
            new { fullName = "Проверка Aspire", phone = "+79991234567" }, token);
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        using var saved = await client.GetAsync(created.Headers.Location, token);
        Assert.Equal(HttpStatusCode.OK, saved.StatusCode);
    }
}
