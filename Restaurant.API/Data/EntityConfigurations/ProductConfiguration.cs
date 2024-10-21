using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.EntityConfigurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(p => p.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(p => p.Name)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .Property(p => p.Description)
            .HasColumnType("text");

        builder
            .Property(p => p.ImageUrl)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .Property(p => p.OldPrice)
            .HasColumnType("numeric(18,2)")
            .IsRequired(false);

        builder
            .Property(p => p.Price)
            .HasColumnType("numeric(18,2)");

        builder
            .HasIndex(p => p.Name)
            .IsUnique();

        builder.Navigation(p => p.Category).AutoInclude();
    }
}