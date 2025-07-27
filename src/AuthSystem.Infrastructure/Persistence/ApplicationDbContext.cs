using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Infrastructure.Persistence;

/// <summary>
/// DbContext اصلی برنامه برای تعامل با پایگاه داده
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="options">گزینه‌های پیکربندی DbContext</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet برای Entity کاربران
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// DbSet برای Entity نقش‌ها
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// DbSet برای Entity مجوزها
    /// </summary>
    public DbSet<Permission> Permissions => Set<Permission>();

    /// <summary>
    /// DbSet برای Entity ارتباط کاربران و نقش‌ها
    /// </summary>
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    /// <summary>
    /// DbSet برای Entity ارتباط نقش‌ها و مجوزها
    /// </summary>
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    /// <summary>
    /// DbSet برای Entity توکن‌های تازه‌سازی
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    /// <summary>
    /// DbSet برای Entity تاریخچه ورود
    /// </summary>
    public DbSet<LoginHistory> LoginHistories => Set<LoginHistory>();

    /// <summary>
    /// DbSet برای Entity دستگاه‌های کاربر
    /// </summary>
    public DbSet<UserDevice> UserDevices => Set<UserDevice>();

    /// <summary>
    /// پیکربندی مدل‌ها و روابط در زمان ایجاد Schema
    /// </summary>
    /// <param name="modelBuilder">سازنده مدل EF Core</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // فراخوانی پیکربندی‌های سفارشی برای هر Entity
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}