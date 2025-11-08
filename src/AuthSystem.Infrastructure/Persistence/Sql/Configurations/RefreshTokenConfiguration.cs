using AuthSystem.Infrastructure.Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Sql.Configurations;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Hash).HasMaxLength(256).IsRequired();
        builder.Property(x => x.UserAgent).HasMaxLength(256);
        builder.Property(x => x.Ip).HasMaxLength(64);
        builder.Property(x => x.TenantId).HasMaxLength(128);
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.ExpiresAtUtc).IsRequired();
        builder.HasIndex(x => new { x.UserId, x.ExpiresAtUtc });
        builder.HasIndex(x => x.Hash).IsUnique();
    }
}