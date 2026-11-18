using AutoService.Infrastructure.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
var connection = builder.Configuration.GetConnectionString("autoservice")
    ?? throw new InvalidOperationException("Не задана строка подключения autoservice.");
builder.Services.AddDbContext<AutoServiceDbContext>(options => options.UseMySQL(connection));
using var host = builder.Build();
await using var scope = host.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<AutoServiceDbContext>();
await context.Database.MigrateAsync();
host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Migrations").LogInformation("Миграции автосервиса применены.");
