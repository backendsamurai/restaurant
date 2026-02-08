using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain;

namespace Restaurant.Persistence.EntityConfigurations;

public sealed class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
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
            .Property(m => m.Description)
            .HasColumnType("text");

        builder
            .Property(m => m.ImageUrl)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .Property(m => m.Price)
            .HasColumnType("numeric(18,2)");

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

        builder.Navigation(m => m.Category).AutoInclude();
    }
}
