using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Persistence.Seeders;
using Restaurant.Domain;

namespace Restaurant.Persistence.EntityConfigurations;

public sealed class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
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
            .HasIndex(p => p.Name)
            .IsUnique();

        // Seed Data
        builder.HasData(ProductCategorySeeder.GetProductCategories());
    }
}
