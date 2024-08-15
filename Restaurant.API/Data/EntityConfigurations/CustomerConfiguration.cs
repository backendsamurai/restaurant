using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.EntityConfigurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder
            .Property(c => c.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(c => c.Name)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .Property(c => c.Email)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .Property(c => c.PasswordHash)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .HasIndex(c => c.Email)
            .IsUnique();
    }
}
