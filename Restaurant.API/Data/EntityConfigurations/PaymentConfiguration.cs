using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.API.Data.ValueConverters;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.EntityConfigurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder
            .Property(p => p.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(p => p.Status)
            .HasColumnType("varchar")
            .HasConversion<PaymentStatusValueConverter>();

        builder
            .Property(p => p.Bill)
            .HasColumnType("numeric(18,2)");

        builder
            .Property(p => p.Tip)
            .HasColumnType("numeric(18,2)")
            .IsRequired(false);

        builder
            .Property(p => p.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()");

        builder
            .Property(p => p.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()");
    }
}
