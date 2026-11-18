using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoService.Tests.Api;

/// <summary>Проверки миграций, ограничений и сохранения данных вне HTTP-процесса.</summary>
public class DatabaseApiTests : ApiTestBase
{
    /// <summary>Повторное применение миграций не дублирует начальные данные.</summary>
    [Fact]
    public async Task Migrations_AreRepeatableAndSeedAllTables()
    {
        await using var context = Database.CreateContext();
        await context.Database.MigrateAsync(TestContext.Current.CancellationToken);
        Assert.Empty(await context.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken));
        Assert.Equal(10, await context.Clients.CountAsync(TestContext.Current.CancellationToken));
        Assert.Equal(12, await context.Cars.CountAsync(TestContext.Current.CancellationToken));
        Assert.Equal(10, await context.Mechanics.CountAsync(TestContext.Current.CancellationToken));
        Assert.Equal(12, await context.WorkTypes.CountAsync(TestContext.Current.CancellationToken));
        Assert.Equal(28, await context.Orders.CountAsync(TestContext.Current.CancellationToken));
    }

    /// <summary>Откат миграции данных оставляет схему, повторный накат восстанавливает данные.</summary>
    [Fact]
    public async Task SeedMigration_CanBeRolledBackAndAppliedAgain()
    {
        await using var context = Database.CreateContext();
        var schema = context.Database.GetMigrations().First();
        await context.GetService<IMigrator>().MigrateAsync(schema, TestContext.Current.CancellationToken);
        Assert.Equal(0, await context.Clients.CountAsync(TestContext.Current.CancellationToken));
        await context.Database.MigrateAsync(TestContext.Current.CancellationToken);
        Assert.Equal(28, await context.Orders.CountAsync(TestContext.Current.CancellationToken));
    }

    /// <summary>Созданная запись видна после запуска нового экземпляра API.</summary>
    [Fact]
    public async Task CreatedClient_SurvivesApiRestart()
    {
        using var response = await Client.PostAsJsonAsync("/api/clients",
            new { fullName = "Новый клиент", phone = "+79991234567" }, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        var id = created.GetProperty("id").GetInt32();
        await using var anotherServer = CreateFactory();
        using var anotherClient = anotherServer.CreateClient();
        var saved = await anotherClient.GetFromJsonAsync<JsonElement>($"/api/clients/{id}", TestContext.Current.CancellationToken);
        Assert.Equal("Новый клиент", saved.GetProperty("fullName").GetString());
    }

    /// <summary>Уникальный индекс защищает от дубликата даже при обходе сервисной проверки.</summary>
    [Fact]
    public async Task Database_RejectsDuplicateRegistrationNumber()
    {
        await using var context = Database.CreateContext();
        var original = await context.Cars.FirstAsync(TestContext.Current.CancellationToken);
        var owner = await context.Clients.FirstAsync(TestContext.Current.CancellationToken);
        context.Cars.Add(new Car
        {
            Id = 0,
            OwnerId = owner.Id,
            Owner = owner,
            RegistrationNumber = original.RegistrationNumber,
            Brand = "Lada",
            Model = "Vesta",
            ManufactureYear = 2020
        });
        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync(TestContext.Current.CancellationToken));
    }
}
