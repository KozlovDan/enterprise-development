using AutoService.Infrastructure.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MySql.Data.MySqlClient;

namespace AutoService.Tests.Fixtures;

/// <summary>Отдельная временная база MySQL для каждого теста.</summary>
public sealed class MySqlTestDatabase : IAsyncDisposable
{
    /// <summary>Строка подключения только к временной базе.</summary>
    public string ConnectionString { get; }

    /// <summary>Выбирает уникальное имя базы на настроенном тестовом сервере.</summary>
    public MySqlTestDatabase()
    {
        var source = Environment.GetEnvironmentVariable("ConnectionStrings__testmysql")
            ?? throw new InvalidOperationException("Для HTTP-тестов задайте ConnectionStrings__testmysql. Нужен MySQL 8.4 и право создавать временные базы.");
        var connection = new MySqlConnectionStringBuilder(source)
        {
            Database = "autoservice_test_" + Guid.NewGuid().ToString("N"),
            // Блокировки миграций MySQL принадлежат соединению и не должны оставаться в пуле.
            Pooling = false
        };
        ConnectionString = connection.ConnectionString;
    }

    /// <summary>Новый контекст, не разделяющий отслеживаемые объекты с сервером.</summary>
    public AutoServiceDbContext CreateContext(params IInterceptor[] interceptors) =>
        new(new DbContextOptionsBuilder<AutoServiceDbContext>().UseMySQL(ConnectionString)
            .AddInterceptors(interceptors).Options);

    /// <summary>Создаёт таблицы и начальные данные настоящими миграциями.</summary>
    public async Task InitializeAsync()
    {
        await using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
    }
}
