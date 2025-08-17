using System;
using System.ComponentModel;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع دسترسی‌های سیستم
/// این enum با Flags طراحی شده تا بتوان چندین دسترسی را ترکیب کرد
/// </summary>
[Flags]
public enum PermissionType : long
{
    /// <summary>
    /// بدون دسترسی
    /// </summary>
    [Description("بدون دسترسی")]
    None = 0,

    // دسترسی‌های کاربران عادی (1-1000)

    /// <summary>
    /// مشاهده پروفایل خود
    /// </summary>
    [Description("مشاهده پروفایل")]
    ViewProfile = 1 << 0, // 1

    /// <summary>
    /// ویرایش پروفایل خود
    /// </summary>
    [Description("ویرایش پروفایل")]
    EditProfile = 1 << 1, // 2

    /// <summary>
    /// تغییر رمز عبور
    /// </summary>
    [Description("تغییر رمز عبور")]
    ChangePassword = 1 << 2, // 4

    /// <summary>
    /// فعال‌سازی احراز هویت دو عاملی
    /// </summary>
    [Description("فعال‌سازی دو عاملی")]
    EnableTwoFactor = 1 << 3, // 8

    /// <summary>
    /// مدیریت جلسات فعال
    /// </summary>
    [Description("مدیریت جلسات")]
    ManageSessions = 1 << 4, // 16

    /// <summary>
    /// مشاهده لاگ فعالیت‌های خود
    /// </summary>
    [Description("مشاهده لاگ فعالیت")]
    ViewActivityLog = 1 << 5, // 32

    // دسترسی‌های مدیریت کاربران (1001-2000)

    /// <summary>
    /// مشاهده لیست کاربران
    /// </summary>
    [Description("مشاهده کاربران")]
    ViewUsers = 1 << 10, // 1024

    /// <summary>
    /// ایجاد کاربر جدید
    /// </summary>
    [Description("ایجاد کاربر")]
    CreateUser = 1 << 11, // 2048

    /// <summary>
    /// ویرایش اطلاعات کاربران
    /// </summary>
    [Description("ویرایش کاربران")]
    EditUser = 1 << 12, // 4096

    /// <summary>
    /// حذف کاربران
    /// </summary>
    [Description("حذف کاربران")]
    DeleteUser = 1 << 13, // 8192

    /// <summary>
    /// فعال/غیرفعال کردن کاربران
    /// </summary>
    [Description("تغییر وضعیت کاربران")]
    ChangeUserStatus = 1 << 14, // 16384

    /// <summary>
    /// بازنشانی رمز عبور کاربران
    /// </summary>
    [Description("بازنشانی رمز کاربران")]
    ResetUserPassword = 1 << 15, // 32768

    /// <summary>
    /// مشاهده جزئیات حساسیت بالای کاربران
    /// </summary>
    [Description("مشاهده اطلاعات حساس")]
    ViewSensitiveData = 1 << 16, // 65536

    // دسترسی‌های مدیریت نقش‌ها (2001-3000)

    /// <summary>
    /// مشاهده نقش‌ها
    /// </summary>
    [Description("مشاهده نقش‌ها")]
    ViewRoles = 1 << 20, // 1048576

    /// <summary>
    /// ایجاد نقش جدید
    /// </summary>
    [Description("ایجاد نقش")]
    CreateRole = 1 << 21, // 2097152

    /// <summary>
    /// ویرایش نقش‌ها
    /// </summary>
    [Description("ویرایش نقش")]
    EditRole = 1 << 22, // 4194304

    /// <summary>
    /// حذف نقش‌ها
    /// </summary>
    [Description("حذف نقش")]
    DeleteRole = 1 << 23, // 8388608

    /// <summary>
    /// تخصیص نقش به کاربران
    /// </summary>
    [Description("تخصیص نقش")]
    AssignRole = 1 << 24, // 16777216

    /// <summary>
    /// مدیریت دسترسی‌های نقش‌ها
    /// </summary>
    [Description("مدیریت دسترسی نقش")]
    ManageRolePermissions = 1 << 25, // 33554432

    // دسترسی‌های امنیتی (3001-4000)

    /// <summary>
    /// مشاهده لاگ‌های امنیتی
    /// </summary>
    [Description("مشاهده لاگ امنیتی")]
    ViewSecurityLogs = 1 << 30, // 1073741824

    /// <summary>
    /// مدیریت IP های مسدود
    /// </summary>
    [Description("مدیریت IP مسدود")]
    ManageBlockedIPs = 1 << 31, // 2147483648

    /// <summary>
    /// مشاهده گزارش‌های امنیتی
    /// </summary>
    [Description("مشاهده گزارش امنیتی")]
    ViewSecurityReports = 1L << 32, // 4294967296

    /// <summary>
    /// تنظیمات امنیتی سیستم
    /// </summary>
    [Description("تنظیمات امنیتی")]
    ConfigureSecurity = 1L << 33, // 8589934592

    // دسترسی‌های سیستمی (4001-5000)

    /// <summary>
    /// مشاهده تنظیمات سیستم
    /// </summary>
    [Description("مشاهده تنظیمات")]
    ViewSettings = 1L << 40, // 1099511627776

    /// <summary>
    /// ویرایش تنظیمات سیستم
    /// </summary>
    [Description("ویرایش تنظیمات")]
    EditSettings = 1L << 41, // 2199023255552

    /// <summary>
    /// پشتیبان‌گیری از سیستم
    /// </summary>
    [Description("پشتیبان‌گیری")]
    BackupSystem = 1L << 42, // 4398046511104

    /// <summary>
    /// بازیابی سیستم
    /// </summary>
    [Description("بازیابی سیستم")]
    RestoreSystem = 1L << 43, // 8796093022208

    /// <summary>
    /// مشاهده لاگ‌های سیستم
    /// </summary>
    [Description("مشاهده لاگ سیستم")]
    ViewSystemLogs = 1L << 44, // 17592186044416

    /// <summary>
    /// پاکسازی لاگ‌ها
    /// </summary>
    [Description("پاکسازی لاگ")]
    PurgeLogs = 1L << 45, // 35184372088832

    // دسترسی‌های API (5001-6000)

    /// <summary>
    /// دسترسی به API
    /// </summary>
    [Description("دسترسی API")]
    AccessAPI = 1L << 50, // 1125899906842624

    /// <summary>
    /// ایجاد کلید API
    /// </summary>
    [Description("ایجاد کلید API")]
    CreateAPIKey = 1L << 51, // 2251799813685248

    /// <summary>
    /// مدیریت کلیدهای API
    /// </summary>
    [Description("مدیریت کلید API")]
    ManageAPIKeys = 1L << 52, // 4503599627370496

    // دسترسی ویژه

    /// <summary>
    /// دسترسی کامل به سیستم
    /// </summary>
    [Description("دسترسی کامل")]
    FullAccess = ~0L // تمام بیت‌ها روشن
}

/// <summary>
/// گروه‌های دسترسی از پیش تعریف شده
/// </summary>
public static class PermissionGroups
{
    /// <summary>
    /// دسترسی‌های پایه کاربر عادی
    /// </summary>
    public const PermissionType BasicUser =
        PermissionType.ViewProfile |
        PermissionType.EditProfile |
        PermissionType.ChangePassword |
        PermissionType.EnableTwoFactor |
        PermissionType.ManageSessions |
        PermissionType.ViewActivityLog;

    /// <summary>
    /// دسترسی‌های مدیر کاربران
    /// </summary>
    public const PermissionType UserManager =
        BasicUser |
        PermissionType.ViewUsers |
        PermissionType.CreateUser |
        PermissionType.EditUser |
        PermissionType.ChangeUserStatus |
        PermissionType.ResetUserPassword;

    /// <summary>
    /// دسترسی‌های مدیر نقش‌ها
    /// </summary>
    public const PermissionType RoleManager =
        PermissionType.ViewRoles |
        PermissionType.CreateRole |
        PermissionType.EditRole |
        PermissionType.DeleteRole |
        PermissionType.AssignRole |
        PermissionType.ManageRolePermissions;

    /// <summary>
    /// دسترسی‌های مدیر امنیت
    /// </summary>
    public const PermissionType SecurityManager =
        PermissionType.ViewSecurityLogs |
        PermissionType.ManageBlockedIPs |
        PermissionType.ViewSecurityReports |
        PermissionType.ConfigureSecurity;

    /// <summary>
    /// دسترسی‌های مدیر سیستم
    /// </summary>
    public const PermissionType SystemAdmin =
        UserManager |
        RoleManager |
        SecurityManager |
        PermissionType.ViewSettings |
        PermissionType.EditSettings |
        PermissionType.ViewSystemLogs |
        PermissionType.BackupSystem |
        PermissionType.RestoreSystem;

    /// <summary>
    /// دسترسی‌های توسعه‌دهنده
    /// </summary>
    public const PermissionType Developer =
        BasicUser |
        PermissionType.AccessAPI |
        PermissionType.CreateAPIKey |
        PermissionType.ManageAPIKeys |
        PermissionType.ViewSystemLogs;
}

/// <summary>
/// متدهای کمکی برای PermissionType
/// </summary>
public static class PermissionTypeExtensions
{
    /// <summary>
    /// بررسی داشتن یک دسترسی خاص
    /// </summary>
    public static bool Has(this PermissionType permissions, PermissionType permission)
    {
        return (permissions & permission) == permission;
    }

    /// <summary>
    /// بررسی داشتن هر یک از دسترسی‌های مورد نظر
    /// </summary>
    public static bool HasAny(this PermissionType permissions, params PermissionType[] checkPermissions)
    {
        foreach (var permission in checkPermissions)
        {
            if ((permissions & permission) == permission)
                return true;
        }
        return false;
    }

    /// <summary>
    /// بررسی داشتن همه دسترسی‌های مورد نظر
    /// </summary>
    public static bool HasAll(this PermissionType permissions, params PermissionType[] checkPermissions)
    {
        foreach (var permission in checkPermissions)
        {
            if ((permissions & permission) != permission)
                return false;
        }
        return true;
    }

    /// <summary>
    /// اضافه کردن دسترسی
    /// </summary>
    public static PermissionType Add(this PermissionType permissions, PermissionType permission)
    {
        return permissions | permission;
    }

    /// <summary>
    /// حذف دسترسی
    /// </summary>
    public static PermissionType Remove(this PermissionType permissions, PermissionType permission)
    {
        return permissions & ~permission;
    }

    /// <summary>
    /// دریافت لیست دسترسی‌های فعال
    /// </summary>
    public static IEnumerable<PermissionType> GetActivePermissions(this PermissionType permissions)
    {
        foreach (PermissionType permission in Enum.GetValues(typeof(PermissionType)))
        {
            if (permission != PermissionType.None &&
                permission != PermissionType.FullAccess &&
                permissions.Has(permission))
            {
                yield return permission;
            }
        }
    }

    /// <summary>
    /// بررسی دسترسی مدیریتی
    /// </summary>
    public static bool IsManager(this PermissionType permissions)
    {
        return permissions.HasAny(
            PermissionType.CreateUser,
            PermissionType.EditUser,
            PermissionType.CreateRole,
            PermissionType.EditRole,
            PermissionType.ConfigureSecurity,
            PermissionType.EditSettings
        );
    }

    /// <summary>
    /// بررسی دسترسی سطح بالا
    /// </summary>
    public static bool IsHighLevel(this PermissionType permissions)
    {
        return permissions.HasAny(
            PermissionType.DeleteUser,
            PermissionType.ViewSensitiveData,
            PermissionType.DeleteRole,
            PermissionType.ConfigureSecurity,
            PermissionType.BackupSystem,
            PermissionType.RestoreSystem,
            PermissionType.PurgeLogs,
            PermissionType.FullAccess
        );
    }

    /// <summary>
    /// دریافت سطح دسترسی
    /// </summary>
    public static int GetAccessLevel(this PermissionType permissions)
    {
        if (permissions == PermissionType.FullAccess) return 100;
        if (permissions.IsHighLevel()) return 80;
        if (permissions.IsManager()) return 60;
        if (permissions.HasAny(PermissionType.ViewUsers, PermissionType.ViewRoles)) return 40;
        if (permissions != PermissionType.None) return 20;
        return 0;
    }
}
