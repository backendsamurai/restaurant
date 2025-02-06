using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.API.Data.ValueConverters;
using Restaurant.Domain;

namespace Restaurant.API.Data.EntityConfigurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .Property(o => o.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Navigation(o => o.Customer).AutoInclude();
        builder.Navigation(o => o.Payment).AutoInclude();
        builder.Navigation(o => o.Items).EnableLazyLoading().AutoInclude();

        builder
            .Property(o => o.Status)
            .HasColumnType("varchar")
            .HasConversion<OrderStatusValueConverter>();

        builder
            .Property(o => o.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()");

        builder
            .Property(o => o.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()");
    }
}
