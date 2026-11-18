using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutoService.Infrastructure.MySql;

/// <summary>Создание контекста для dotnet ef без запуска API.</summary>
public class AutoServiceDbContextFactory : IDesignTimeDbContextFactory<AutoServiceDbContext>
{
    /// <inheritdoc />
    public AutoServiceDbContext CreateDbContext(string[] args)
    {
        var connection = Environment.GetEnvironmentVariable("ConnectionStrings__autoservice")
            ?? throw new InvalidOperationException("Укажите ConnectionStrings__autoservice для dotnet ef.");
        return new(new DbContextOptionsBuilder<AutoServiceDbContext>().UseMySQL(connection).Options);
    }
}
