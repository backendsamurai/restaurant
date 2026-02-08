using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain;

namespace Restaurant.Persistence.EntityConfigurations;

public sealed class ConsumerConfiguration : IEntityTypeConfiguration<Consumer>
{
    public void Configure(EntityTypeBuilder<Consumer> builder)
    {
        builder
            .Property(c => c.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(c => c.CreatedAtUtc)
            .HasColumnType("timestamptz");

        builder
            .Property(c => c.UpdatedAtUtc)
            .HasColumnType("timestamptz");

        builder
            .Property(c => c.DeletedAtUtc)
            .HasColumnType("timestamptz");
    }
}
