using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// تنظیمات Entity RefreshToken برای EF Core
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        // Index
        builder.HasIndex(rt => rt.Token).IsUnique();

        // ویژگی‌ها
        builder.Property(rt => rt.Token)
            .IsRequired();

        builder.Property(rt => rt.IpAddress)
            .HasMaxLength(50);

        builder.Property(rt => rt.UserAgent)
            .HasMaxLength(255);

        // روابط
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}