using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// پیکربندی Entity کاربر
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// پیکربندی Entity کاربر
    /// </summary>
    /// <param name="builder">سازنده Entity</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // نام جدول
        builder.ToTable("Users");

        // پیکربندی فیلدهای اجباری و محدودیت‌ها
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.ProfileImageUrl)
            .HasMaxLength(500); // URL معمولاً طولانی‌تر است

        // ایجاد Index برای فیلدهای Unik
        builder.HasIndex(u => u.UserName)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique(); // اگر بخواهیم شماره تلفن هم Unik باشد

        // پیکربندی روابط
        // یک کاربر می‌تواند چند نقش داشته باشد (Many-to-Many با UserRole)
        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade); // حذف کاربر -> حذف نقش‌هایش

        // یک کاربر می‌تواند چند توکن تازه‌سازی داشته باشد
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // یک کاربر می‌تواند چند تاریخچه ورود داشته باشد
        builder.HasMany(u => u.LoginHistories)
            .WithOne(lh => lh.User)
            .HasForeignKey(lh => lh.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // یک کاربر می‌تواند چند دستگاه داشته باشد
        builder.HasMany(u => u.UserDevices)
            .WithOne(ud => ud.User)
            .HasForeignKey(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}