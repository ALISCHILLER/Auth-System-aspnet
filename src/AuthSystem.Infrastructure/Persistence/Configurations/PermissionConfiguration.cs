using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// پیکربندی Entity مجوز
/// </summary>
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <summary>
    /// پیکربندی Entity مجوز
    /// </summary>
    /// <param name="builder">سازنده Entity</param>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // نام جدول
        builder.ToTable("Permissions");

        // پیکربندی فیلدهای اجباری و محدودیت‌ها
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(255);

        // ایجاد Index برای فیلد Unik
        builder.HasIndex(p => p.Name)
            .IsUnique();

        // پیکربندی روابط
        // یک مجوز می‌تواند به چند نقش تعلق داشته باشد (Many-to-Many با RolePermission)
        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}