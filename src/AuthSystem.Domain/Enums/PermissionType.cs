namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع مجوزها
/// این enum برای طبقه‌بندی مجوزها در سیستم استفاده می‌شود
/// </summary>
public enum PermissionType
{
    /// <summary>
    /// مجوز خواندن (Read)
    /// </summary>
    Read = 1,

    /// <summary>
    /// مجوز نوشتن (Write)
    /// </summary>
    Write = 2,

    /// <summary>
    /// مجوز حذف (Delete)
    /// </summary>
    Delete = 3,

    /// <summary>
    /// مجوز ایجاد (Create)
    /// </summary>
    Create = 4,

    /// <summary>
    /// مجوز ادمین (Admin)
    /// </summary>
    Admin = 5,

    /// <summary>
    /// مجوز مدیریت کاربران (UserManagement)
    /// </summary>
    UserManagement = 6,

    /// <summary>
    /// مجوز مدیریت محتوا (ContentManagement)
    /// </summary>
    ContentManagement = 7,

    /// <summary>
    /// مجوز گزارش‌گیری (Reporting)
    /// </summary>
    Reporting = 8,

    /// <summary>
    /// مجوز تنظیمات سیستم (SystemSettings)
    /// </summary>
    SystemSettings = 9,

    /// <summary>
    /// مجوز مدیریت امنیت (SecurityManagement)
    /// </summary>
    SecurityManagement = 10,

    /// <summary>
    /// مجوز ایجاد کاربر جدید (UserCreate)
    /// </summary>
    UserCreate = 11,

    /// <summary>
    /// مجوز ایجاد نقش جدید (RoleCreate)
    /// </summary>
    RoleCreate = 12,

    /// <summary>
    /// مجوز تخصیص نقش به کاربر (RoleAssign)
    /// </summary>
    RoleAssign = 13
}