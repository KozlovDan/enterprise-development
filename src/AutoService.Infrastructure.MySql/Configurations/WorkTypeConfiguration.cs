using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.MySql.Configurations;

/// <summary>Длина названия, точность стоимости и допустимая продолжительность вида работ.</summary>
public class WorkTypeConfiguration : IEntityTypeConfiguration<WorkType>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<WorkType> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id).ValueGeneratedOnAdd();
        builder.Property(entity => entity.Name).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.Cost).HasPrecision(12, 2);
        builder.Property(entity => entity.Duration).HasConversion(value => value.Ticks, value => TimeSpan.FromTicks(value));
        builder.ToTable(table =>
        {
            table.HasCheckConstraint("CK_WorkTypes_Cost", "`Cost` > 0 AND `Cost` <= 1000000000");
            table.HasCheckConstraint("CK_WorkTypes_Duration", "`Duration` BETWEEN 600000000 AND 315360000000000");
            table.HasCheckConstraint("CK_WorkTypes_Category", "`Category` BETWEEN 1 AND 6");
        });
    }
}
