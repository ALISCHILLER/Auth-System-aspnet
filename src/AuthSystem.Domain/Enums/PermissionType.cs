// File: AuthSystem.Domain/Enums/PermissionType.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع مجوزها در سیستم
/// - این enum برای تمایز بین انواع مجوزها استفاده می‌شود
/// </summary>
public enum PermissionType
{
    /// <summary>
    /// مجوز خواندن
    /// </summary>
    Read = 1,

    /// <summary>
    /// مجوز نوشتن
    /// </summary>
    Write = 2,

    /// <summary>
    /// مجوز حذف
    /// </summary>
    Delete = 3,

    /// <summary>
    /// مجوز ایجاد
    /// </summary>
    Create = 4,

    /// <summary>
    /// مجوز مدیریت
    /// </summary>
    Manage = 5,

    /// <summary>
    /// مجوز ادمین
    /// </summary>
    Admin = 6,

    /// <summary>
    /// مجوز احراز هویت
    /// </summary>
    Authenticate = 7,

    /// <summary>
    /// مجوز تأیید
    /// </summary>
    Verify = 8,

    /// <summary>
    /// مجوز گزارش‌گیری
    /// </summary>
    Report = 9
}