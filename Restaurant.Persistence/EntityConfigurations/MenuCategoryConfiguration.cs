using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain;

namespace Restaurant.Persistence.EntityConfigurations;

public sealed class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
{
    public void Configure(EntityTypeBuilder<MenuCategory> builder)
    {
        builder
            .Property(m => m.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(m => m.Name)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .Property(m => m.CreatedAtUtc)
            .HasColumnType("timestamptz");

        builder
            .Property(m => m.UpdatedAtUtc)
            .HasColumnType("timestamptz");

        builder
            .Property(m => m.DeletedAtUtc)
            .HasColumnType("timestamptz");

        builder
            .HasIndex(m => m.Name)
            .IsUnique();

        // Seed Data
        builder.HasData(GetMenuCategories());
    }

    public static MenuCategory[] GetMenuCategories()
    {
        return [
            new MenuCategory(Guid.Parse("04a8f982-b59f-41a8-b119-db5acdf96700"), "Drinks"),
            new MenuCategory(Guid.Parse("3f468941-50ae-4bdf-aea4-47fdfd4cbb1e"), "Steaks"),
            new MenuCategory(Guid.Parse("50c1ab12-22f6-4c9e-8895-dd997e6286dd"), "Barbecue"),
            new MenuCategory(Guid.Parse("7c9883c4-a24a-4403-8588-5972ba14a90e"), "Desserts"),
            new MenuCategory(Guid.Parse("7cff41a6-125b-44c9-8a02-98a3808885fb"), "Coffee"),
            new MenuCategory(Guid.Parse("a3f97e6b-6fe9-4c52-bfe7-29fcedf10246"), "Hot Dogs"),
            new MenuCategory(Guid.Parse("c9c2d401-12d9-40ca-befa-2ffc70449e30"), "Seafood"),
            new MenuCategory(Guid.Parse("fbb342e6-910e-493c-93c7-5b1b58f69cd7"), "Fast Food"),
            new MenuCategory(Guid.Parse("fc32339e-b6cc-44e0-80cb-f0e38fc12d6b"), "Pizzas")
        ];
    }
}
