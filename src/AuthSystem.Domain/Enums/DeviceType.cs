// File: AuthSystem.Domain/Enums/DeviceType.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع دستگاه‌هایی که کاربر ممکن است از آن وارد سیستم شود
/// - این enum برای ردیابی و مدیریت دستگاه‌های کاربر استفاده می‌شود
/// </summary>
public enum DeviceType
{
    /// <summary>
    /// دستگاه ناشناخته
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// رایانه رومیزی
    /// </summary>
    Desktop = 1,

    /// <summary>
    /// تلفن همراه
    /// </summary>
    Mobile = 2,

    /// <summary>
    /// تبلت
    /// </summary>
    Tablet = 3,

    /// <summary>
    /// ربات یا اسکرپر
    /// </summary>
    Bot = 4,

    /// <summary>
    /// دستگاه IoT
    /// </summary>
    IoT = 5,

    /// <summary>
    /// سرور یا اپلیکیشن سرور
    /// </summary>
    Server = 6
}