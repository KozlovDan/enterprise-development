using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.MySql.Configurations;

/// <summary>Настройки таблицы Car.</summary>
public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id).ValueGeneratedOnAdd();
        builder.Property(entity => entity.RegistrationNumber).HasMaxLength(15).IsRequired();
        builder.Property(entity => entity.Brand).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.Model).HasMaxLength(100).IsRequired();
        builder.HasIndex(entity => entity.RegistrationNumber).IsUnique();
        builder.HasOne(entity => entity.Owner).WithMany().HasForeignKey(entity => entity.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.ToTable(table => table.HasCheckConstraint("CK_Cars_ManufactureYear", "`ManufactureYear` BETWEEN 1886 AND 9999"));
    }
}
