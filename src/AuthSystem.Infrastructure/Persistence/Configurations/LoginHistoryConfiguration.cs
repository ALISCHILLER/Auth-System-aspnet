using AuthSystem.Domain.Entities.Security.LoginHistory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// تنظیمات Entity LoginHistory برای EF Core
/// </summary>
public class LoginHistoryConfiguration : IEntityTypeConfiguration<LoginHistory>
{
    public void Configure(EntityTypeBuilder<LoginHistory> builder)
    {
        builder.HasKey(lh => lh.Id);

        // ویژگی‌ها
        builder.Property(lh => lh.IpAddress)
            .HasMaxLength(50);

        builder.Property(lh => lh.UserAgent)
            .HasMaxLength(255);

        builder.Property(lh => lh.FailureReason)
            .HasMaxLength(255);

        // روابط
        builder.HasOne(lh => lh.User)
            .WithMany(u => u.LoginHistories)
            .HasForeignKey(lh => lh.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}