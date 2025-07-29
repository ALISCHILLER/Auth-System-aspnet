using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// تنظیمات Entity User برای EF Core
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        // Indexها
        builder.HasIndex(u => u.EmailAddress).IsUnique();
        builder.HasIndex(u => u.UserName).IsUnique();

        // ویژگی‌ها
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.EmailAddress)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.ProfileImageUrl)
            .HasMaxLength(500);

        builder.Property(u => u.NationalCode)
            .HasMaxLength(10);

        // روابط
        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.LoginHistories)
            .WithOne(lh => lh.User)
            .HasForeignKey(lh => lh.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.UserDevices)
            .WithOne(ud => ud.User)
            .HasForeignKey(ud => ud.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}