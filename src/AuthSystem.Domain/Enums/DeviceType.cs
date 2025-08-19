namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع دستگاه‌ها
/// این enum برای تشخیص نوع دستگاه کاربر استفاده می‌شود
/// </summary>
public enum DeviceType
{
    /// <summary>
    /// دستگاه دسکتاپ (ویندوز، مک، لینوکس)
    /// </summary>
    Desktop = 1,

    /// <summary>
    /// دستگاه موبایل (اندروید، آیفون)
    /// </summary>
    Mobile = 2,

    /// <summary>
    /// تبلت (آیپد، تبلت اندروید)
    /// </summary>
    Tablet = 3,

    /// <summary>
    /// ربات یا اسکرپر (Googlebot, Facebookbot)
    /// </summary>
    Bot = 4,

    /// <summary>
    /// دستگاه IoT (دستگاه‌های اینترنت اشیاء)
    /// </summary>
    IoT = 5,

    /// <summary>
    /// دستگاه مجهول
    /// </summary>
    Unknown = 6
}