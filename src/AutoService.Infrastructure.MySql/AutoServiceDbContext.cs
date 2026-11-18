using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.MySql;

/// <summary>Связи и ограничения базы автосервиса.</summary>
/// <param name="options">Настройки подключения MySQL.</param>
public class AutoServiceDbContext(DbContextOptions<AutoServiceDbContext> options) : DbContext(options)
{
    /// <summary>Клиенты.</summary>
    public DbSet<Client> Clients => Set<Client>();
    /// <summary>Автомобили.</summary>
    public DbSet<Car> Cars => Set<Car>();
    /// <summary>Механики.</summary>
    public DbSet<Mechanic> Mechanics => Set<Mechanic>();
    /// <summary>Виды работ.</summary>
    public DbSet<WorkType> WorkTypes => Set<WorkType>();
    /// <summary>Заказы.</summary>
    public DbSet<Order> Orders => Set<Order>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Person>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoServiceDbContext).Assembly);
        SeedData.Configure(modelBuilder);
    }
}
