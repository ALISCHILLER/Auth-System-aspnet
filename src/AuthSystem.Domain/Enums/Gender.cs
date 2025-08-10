using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// جنسیت کاربران سیستم
/// این enum برای ذخیره و مدیریت جنسیت کاربران استفاده می‌شود
/// </summary>
public enum Gender
{
    /// <summary>
    /// جنسیت نامشخص است
    /// این مقدار زمانی استفاده می‌شود که کاربر جنسیت خود را انتخاب نکرده باشد
    /// </summary>
    [Display(Name = "نامشخص", Description = "جنسیت انتخاب نشده")]
    Unknown = 0,

    /// <summary>
    /// جنسیت مرد
    /// </summary>
    [Display(Name = "مرد", Description = "جنسیت مرد")]
    Male = 1,

    /// <summary>
    /// جنسیت زن
    /// </summary>
    [Display(Name = "زن", Description = "جنسیت زن")]
    Female = 2,
}