using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع مجوزهای سیستم
/// این enum برای دسته‌بندی مجوزهای کاربران و نقش‌ها استفاده می‌شود
/// </summary>
public enum PermissionType
{
    /// <summary>
    /// مجوز خواندن
    /// </summary>
    [Display(Name = "خواندن", Description = "اجازه خواندن داده‌ها")]
    Read = 0,

    /// <summary>
    /// مجوز نوشتن
    /// </summary>
    [Display(Name = "نوشتن", Description = "اجازه ایجاد یا ویرایش داده‌ها")]
    Write = 1,

    /// <summary>
    /// مجوز حذف
    /// </summary>
    [Display(Name = "حذف", Description = "اجازه حذف داده‌ها")]
    Delete = 2,

    /// <summary>
    /// مجوز ادمین
    /// </summary>
    [Display(Name = "ادمین", Description = "اجازه مدیریت کامل سیستم")]
    Admin = 3
}