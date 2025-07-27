using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// پیکربندی Entity ارتباط Many-to-Many نقش و مجوز
/// </summary>
public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    /// <summary>
    /// پیکربندی Entity ارتباط نقش و مجوز
    /// </summary>
    /// <param name="builder">سازنده Entity</param>
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // نام جدول
        builder.ToTable("RolePermissions");

        // ترکیب RoleId و PermissionId کلید اصلی (Composite Key) تشکیل می‌دهند
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        // پیکربندی روابط
        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}