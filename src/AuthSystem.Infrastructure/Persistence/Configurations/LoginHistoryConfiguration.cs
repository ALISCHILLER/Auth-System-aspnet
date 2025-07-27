using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// پیکربندی Entity تاریخچه ورود
/// </summary>
public class LoginHistoryConfiguration : IEntityTypeConfiguration<LoginHistory>
{
    /// <summary>
    /// پیکربندی Entity تاریخچه ورود
    /// </summary>
    /// <param name="builder">سازنده Entity</param>
    public void Configure(EntityTypeBuilder<LoginHistory> builder)
    {
        // نام جدول
        builder.ToTable("LoginHistories");

        // پیکربندی فیلدهای اختیاری
        builder.Property(lh => lh.IpAddress)
            .HasMaxLength(50);

        builder.Property(lh => lh.UserAgent)
            .HasMaxLength(500);

        builder.Property(lh => lh.FailureReason)
            .HasMaxLength(500);

        // پیکربندی روابط
        builder.HasOne(lh => lh.User)
            .WithMany(u => u.LoginHistories)
            .HasForeignKey(lh => lh.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}