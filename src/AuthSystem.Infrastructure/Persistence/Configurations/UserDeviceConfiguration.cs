using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// تنظیمات Entity UserDevice برای EF Core
/// </summary>
public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.HasKey(ud => ud.Id);

        // Index
        builder.HasIndex(ud => ud.DeviceId).IsUnique();

        // ویژگی‌ها
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

        // روابط
        builder.HasOne(ud => ud.User)
            .WithMany(u => u.UserDevices)
            .HasForeignKey(ud => ud.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}