using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Configurations;

/// <summary>
/// پیکربندی Entity نقش
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// پیکربندی Entity نقش
    /// </summary>
    /// <param name="builder">سازنده Entity</param>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // نام جدول
        builder.ToTable("Roles");

        // پیکربندی فیلدهای اجباری و محدودیت‌ها
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Description)
            .HasMaxLength(255);

        // ایجاد Index برای فیلد Unik
        builder.HasIndex(r => r.Name)
            .IsUnique();

        // پیکربندی روابط
        // یک نقش می‌تواند به چند کاربر تعلق داشته باشد (Many-to-Many با UserRole)
        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // یک نقش می‌تواند چند مجوز داشته باشد (Many-to-Many با RolePermission)
        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}