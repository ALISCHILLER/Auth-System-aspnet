namespace AuthSystem.Domain.Enums;

/// <summary>
/// وضعیت‌های مختلف احراز هویت در سیستم
/// این enum برای نمایش وضعیت عملیات احراز هویت استفاده می‌شود
/// </summary>
public enum AuthStatus
{
    /// <summary>
    /// عملیات احراز هویت با موفقیت انجام شد
    /// </summary>
    Success,

    /// <summary>
    /// اعتبارسنجی ناموفق بود (نام کاربری یا رمز عبور اشتباه)
    /// </summary>
    InvalidCredentials,

    /// <summary>
    /// حساب کاربری به دلیل تلاش‌های مکرر ناموفق قفل شده است
    /// </summary>
    AccountLocked,

    /// <summary>
    /// ایمیل کاربر تأیید نشده است
    /// </summary>
    EmailNotConfirmed,

    /// <summary>
    /// شماره تلفن کاربر تأیید نشده است
    /// </summary>
    PhoneNotConfirmed,

    /// <summary>
    /// توکن تأیید کننده منقضی شده است
    /// </summary>
    TokenExpired,

    /// <summary>
    /// توکن تأیید کننده نامعتبر است
    /// </summary>
    InvalidToken
}