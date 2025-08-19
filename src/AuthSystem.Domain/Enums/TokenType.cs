namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع توکن در سیستم
/// این enum برای طبقه‌بندی توکن‌های امنیتی استفاده می‌شود
/// </summary>
public enum TokenType
{
    /// <summary>
    /// توکن دسترسی (برای دسترسی به APIها)
    /// </summary>
    Access = 1,

    /// <summary>
    /// توکن نوسازی (برای دریافت توکن دسترسی جدید)
    /// </summary>
    Refresh = 2,

    /// <summary>
    /// توکن تایید ایمیل (برای تأیید آدرس ایمیل)
    /// </summary>
    EmailVerification = 3,

    /// <summary>
    /// توکن بازیابی رمز عبور (برای تغییر رمز عبور)
    /// </summary>
    PasswordReset = 4,

    /// <summary>
    /// توکن احراز هویت دو عاملی (برای تأیید هویت دو مرحله‌ای)
    /// </summary>
    TwoFactor = 5,

    /// <summary>
    /// کلید API (برای دسترسی سرور به سرور)
    /// </summary>
    ApiKey = 6,

    /// <summary>
    /// توکن جلسه (برای مدیریت جلسات کاربر)
    /// </summary>
    Session = 7,

    /// <summary>
    /// توکن تایید تراکنش (برای تأیید عملیات حساس)
    /// </summary>
    Transaction = 8,

    /// <summary>
    /// توکن احراز هویت اجتماعی (برای احراز هویت با شبکه‌های اجتماعی)
    /// </summary>
    SocialAuth = 9
}