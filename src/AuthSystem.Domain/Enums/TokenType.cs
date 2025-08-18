// File: AuthSystem.Domain/Enums/TokenType.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع توکن در سیستم
/// - این enum برای تمایز بین انواع توکن‌های امنیتی استفاده می‌شود
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
    /// توکن تأیید ایمیل
    /// </summary>
    EmailVerification = 3,

    /// <summary>
    /// توکن بازیابی رمز عبور
    /// </summary>
    PasswordReset = 4,

    /// <summary>
    /// توکن احراز هویت دو عاملی
    /// </summary>
    TwoFactor = 5,

    /// <summary>
    /// کلید API
    /// </summary>
    ApiKey = 6,

    /// <summary>
    /// توکن جلسه
    /// </summary>
    Session = 7,

    /// <summary>
    /// توکن تأیید شماره تلفن
    /// </summary>
    PhoneVerification = 8
}