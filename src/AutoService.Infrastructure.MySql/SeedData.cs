using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.MySql;

/// <summary>Фиксированные начальные записи, включаемые в миграции EF Core.</summary>
internal static class SeedData
{
    internal static void Configure(ModelBuilder modelBuilder)
    {
        var seed = new AutoServiceSeed();
        modelBuilder.Entity<Client>().HasData(seed.Clients.Select(client => new
        {
            client.Id,
            client.FullName,
            client.Phone
        }));
        modelBuilder.Entity<Car>().HasData(seed.Cars.Select(car => new
        {
            car.Id,
            car.RegistrationNumber,
            car.Brand,
            car.Model,
            car.ManufactureYear,
            car.OwnerId
        }));
        modelBuilder.Entity<Mechanic>().HasData(seed.Mechanics.Select(mechanic => new
        {
            mechanic.Id,
            mechanic.FullName,
            mechanic.PassportNumber,
            mechanic.Specialization,
            mechanic.ExperienceYears
        }));
        modelBuilder.Entity<WorkType>().HasData(seed.WorkTypes.Select(work => new
        {
            work.Id,
            work.Name,
            work.Category,
            work.Cost,
            work.Duration
        }));
        modelBuilder.Entity<Order>().HasData(seed.Orders.Select(order => new
        {
            order.Id,
            order.CarId,
            order.ClientId,
            order.MechanicId,
            order.AcceptedAt,
            order.ReturnedAt
        }));
        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("OrderWorks").HasData(
            seed.Orders.SelectMany(order => order.WorkTypes.Select(work => new
            {
                OrderId = order.Id,
                WorkTypeId = work.Id
            })));
    }
}
