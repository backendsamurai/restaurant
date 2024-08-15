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
    }
}
