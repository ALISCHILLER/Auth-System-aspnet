using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// پیکربندی Entity توکن تازه‌سازی
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    /// <summary>
    /// پیکربندی Entity توکن تازه‌سازی
    /// </summary>
    /// <param name="builder">سازنده Entity</param>
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // نام جدول
        builder.ToTable("RefreshTokens");

        // پیکربندی فیلدهای اجباری و محدودیت‌ها
        builder.Property(rt => rt.Token)
            .IsRequired();

        // ایجاد Index برای فیلد Unik
        builder.HasIndex(rt => rt.Token)
            .IsUnique();

        // پیکربندی روابط
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}