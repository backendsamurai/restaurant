using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.API.Data.Seeders;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.EntityConfigurations;

public sealed class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
{
    public void Configure(EntityTypeBuilder<EmployeeRole> builder)
    {
        builder
            .Property(e => e.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(e => e.Name)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .HasIndex(e => e.Name)
            .IsUnique();

        // Seed Data
        builder.HasData(EmployeeRoleSeeder.employeeRoles);
    }
}