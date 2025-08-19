namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع خطا در احراز هویت دو عاملی
/// این enum برای طبقه‌بندی خطاها در فرآیند 2FA استفاده می‌شود
/// </summary>
public enum TwoFactorErrorType
{
    /// <summary>
    /// کد وارد شده نامعتبر است
    /// </summary>
    InvalidCode = 1,

    /// <summary>
    /// کد منقضی شده است
    /// </summary>
    ExpiredCode = 2,

    /// <summary>
    /// تعداد تلاش‌ها بیش از حد مجاز است
    /// </summary>
    TooManyAttempts = 3,

    /// <summary>
    /// کلید محرمانه نامعتبر است
    /// </summary>
    InvalidSecretKey = 4,

    /// <summary>
    /// کلید محرمانه فعال نیست
    /// </summary>
    InactiveSecretKey = 5,

    /// <summary>
    /// دستگاه شناخته شده نیست
    /// </summary>
    UnknownDevice = 6,

    /// <summary>
    /// درخواست تکراری برای کد جدید
    /// </summary>
    RequestThrottled = 7,

    /// <summary>
    /// روش احراز هویت دو عاملی فعال نیست
    /// </summary>
    TwoFactorNotEnabled = 8,

    /// <summary>
    /// خطا در ارسال کد
    /// </summary>
    DeliveryFailed = 9
}