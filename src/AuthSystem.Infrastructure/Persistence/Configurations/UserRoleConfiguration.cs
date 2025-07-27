using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// پیکربندی Entity ارتباط Many-to-Many کاربر و نقش
/// </summary>
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <summary>
    /// پیکربندی Entity ارتباط کاربر و نقش
    /// </summary>
    /// <param name="builder">سازنده Entity</param>
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // نام جدول
        builder.ToTable("UserRoles");

        // ترکیب UserId و RoleId کلید اصلی (Composite Key) تشکیل می‌دهند
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        // پیکربندی روابط
        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}