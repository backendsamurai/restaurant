using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.EntityConfigurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .Property(e => e.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");
    }
}