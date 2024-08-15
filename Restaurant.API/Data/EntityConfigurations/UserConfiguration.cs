using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.API.Data.ValueConverters;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.EntityConfigurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(u => u.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(u => u.Name)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .Property(u => u.Email)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .Property(u => u.IsVerified)
            .HasColumnType("boolean")
            .HasDefaultValue(false);

        builder
            .Property(u => u.Role)
            .HasColumnType("varchar")
            .HasConversion<UserRoleValueConverter>();

        builder
            .Property(u => u.PasswordHash)
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
