using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// پیکربندی Entity دستگاه کاربر
/// </summary>
public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    /// <summary>
    /// پیکربندی Entity دستگاه کاربر
    /// </summary>
    /// <param name="builder">سازنده Entity</param>
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        // نام جدول
        builder.ToTable("UserDevices");

        // پیکربندی فیلدهای اجباری و محدودیت‌ها
        builder.Property(ud => ud.DeviceId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ud => ud.DeviceName)
            .HasMaxLength(100);

        builder.Property(ud => ud.DeviceType)
            .HasMaxLength(50);

        builder.Property(ud => ud.OsName)
            .HasMaxLength(50);

        builder.Property(ud => ud.BrowserInfo)
            .HasMaxLength(255);

        builder.Property(ud => ud.IpAddress)
            .HasMaxLength(50);

        // ایجاد Index برای فیلد Unik
        builder.HasIndex(ud => new { ud.UserId, ud.DeviceId })
            .IsUnique(); // هر کاربر فقط یک بار می‌تواند یک DeviceId داشته باشد

        // پیکربندی روابط
        builder.HasOne(ud => ud.User)
            .WithMany(u => u.UserDevices)
            .HasForeignKey(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}