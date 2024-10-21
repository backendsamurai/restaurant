using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.EntityConfigurations;

public sealed class OrderLineItemConfiguration : IEntityTypeConfiguration<OrderLineItem>
{
    public void Configure(EntityTypeBuilder<OrderLineItem> builder)
    {
        builder
            .Property(o => o.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasOne(o => o.Product).WithOne().HasForeignKey<OrderLineItem>(o => o.ProductId).IsRequired();
        builder.Navigation(o => o.Product).AutoInclude();

        builder
            .Property(o => o.Count)
            .HasColumnType("integer");
    }
}
