using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain;
using Restaurant.Persistence.ValueConverters;

namespace Restaurant.Persistence.EntityConfigurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .Property(o => o.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Navigation(o => o.Consumer).AutoInclude();
        builder.Navigation(o => o.Items).EnableLazyLoading().AutoInclude();

        builder
            .Property(o => o.Status)
            .HasColumnType("varchar")
            .HasConversion<OrderStatusValueConverter>();

        builder
            .Property(o => o.CreatedAtUtc)
            .HasColumnType("timestamptz");

        builder
            .Property(o => o.UpdatedAtUtc)
            .HasColumnType("timestamptz");
    }
}
