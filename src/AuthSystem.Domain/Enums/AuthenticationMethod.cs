namespace AuthSystem.Domain.Enums;

/// <summary>
/// روش‌های احراز هویت پشتیبانی شده
/// این enum تعیین می‌کند که کاربر چگونه می‌تواند وارد سیستم شود
/// </summary>
public enum AuthenticationMethod
{
    /// <summary>
    /// احراز هویت با نام کاربری و رمز عبور
    /// </summary>
    UsernamePassword = 1,

    /// <summary>
    /// احراز هویت با ایمیل و رمز عبور
    /// </summary>
    EmailPassword = 2,

    /// <summary>
    /// احراز هویت با شماره تلفن و کد تأیید
    /// </summary>
    PhoneOtp = 3,

    /// <summary>
    /// احراز هویت با ایمیل و کد تأیید
    /// </summary>
    EmailOtp = 4,

    /// <summary>
    /// احراز هویت با احراز هویت دو عاملی
    /// </summary>
    TwoFactorAuth = 5,

    /// <summary>
    /// احراز هویت با شبکه‌های اجتماعی (Google, Facebook و غیره)
    /// </summary>
    Social = 6,

    /// <summary>
    /// احراز هویت با کلید API
    /// </summary>
    ApiKey = 7,

    /// <summary>
    /// احراز هویت با گواهی دیجیتال
    /// </summary>
    Certificate = 8,

    /// <summary>
    /// احراز هویت با روش‌های بیومتریک
    /// </summary>
    Biometric = 9
}