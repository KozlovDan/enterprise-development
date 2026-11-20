using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.MySql.Configurations;

/// <summary>Уникальность паспорта, допустимые специализации и стаж механика.</summary>
public class MechanicConfiguration : IEntityTypeConfiguration<Mechanic>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Mechanic> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id).ValueGeneratedOnAdd();
        builder.HasBaseType((Type?)null);
        builder.Property(entity => entity.FullName).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.PassportNumber).HasMaxLength(11).IsRequired();
        builder.HasIndex(entity => entity.PassportNumber).IsUnique();
        builder.ToTable(table =>
        {
            table.HasCheckConstraint("CK_Mechanics_ExperienceYears", "`ExperienceYears` BETWEEN 0 AND 80");
            table.HasCheckConstraint("CK_Mechanics_Specialization", "`Specialization` BETWEEN 1 AND 6");
        });
    }
}
