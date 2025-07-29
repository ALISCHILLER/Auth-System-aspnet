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
    Unknown,

    /// <summary>
    /// جنسیت مرد
    /// </summary>
    Male,

    /// <summary>
    /// جنسیت زن
    /// </summary>
    Female
}