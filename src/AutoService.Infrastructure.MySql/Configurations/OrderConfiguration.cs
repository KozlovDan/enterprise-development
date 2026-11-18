using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.MySql.Configurations;

/// <summary>Настройки таблицы Order.</summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id).ValueGeneratedOnAdd();
        builder.HasOne(entity => entity.Car).WithMany().HasForeignKey(entity => entity.CarId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(entity => entity.Client).WithMany().HasForeignKey(entity => entity.ClientId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(entity => entity.Mechanic).WithMany().HasForeignKey(entity => entity.MechanicId).OnDelete(DeleteBehavior.Restrict);
        builder.Property(entity => entity.AcceptedAt).HasConversion(value => value.UtcDateTime, value => new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc)));
        builder.Property(entity => entity.ReturnedAt).HasConversion(value => value!.Value.UtcDateTime, value => new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc)));
        builder.HasIndex(entity => new { entity.ClientId, entity.AcceptedAt, entity.Id });
        builder.ToTable(table => table.HasCheckConstraint("CK_Orders_Dates", "`ReturnedAt` IS NULL OR `ReturnedAt` >= `AcceptedAt`"));
        builder.HasMany(entity => entity.WorkTypes).WithMany().UsingEntity<Dictionary<string, object>>(
            "OrderWorks",
            right => right.HasOne<WorkType>().WithMany().HasForeignKey("WorkTypeId").OnDelete(DeleteBehavior.Restrict),
            left => left.HasOne<Order>().WithMany().HasForeignKey("OrderId").OnDelete(DeleteBehavior.Cascade),
            join => join.HasKey("OrderId", "WorkTypeId"));
    }
}
