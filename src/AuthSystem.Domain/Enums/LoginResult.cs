// File: AuthSystem.Domain/Enums/LoginResult.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// نتایج ممکن برای عملیات ورود به سیستم
/// - این enum برای بازگشت نتیجه از سرویس ورود استفاده می‌شود
/// </summary>
public enum LoginResult
{
    /// <summary>
    /// ورود موفقیت‌آمیز
    /// </summary>
    Success = 1,

    /// <summary>
    /// ایمیل یا نام کاربری اشتباه
    /// </summary>
    InvalidCredentials = 2,

    /// <summary>
    /// کاربر غیرفعال است
    /// </summary>
    UserInactive = 3,

    /// <summary>
    /// کاربر قفل شده است
    /// </summary>
    UserLocked = 4,

    /// <summary>
    /// نیاز به تأیید دو مرحله‌ای
    /// </summary>
    TwoFactorRequired = 5,

    /// <summary>
    /// نیاز به تأیید ایمیل
    /// </summary>
    EmailVerificationRequired = 6,

    /// <summary>
    /// تلاش‌های بیش از حد
    /// </summary>
    TooManyAttempts = 7,

    /// <summary>
    /// ایمیل تأیید نشده
    /// </summary>
    EmailNotVerified = 8,

    /// <summary>
    /// حساب کاربری حذف شده است
    /// </summary>
    AccountDeleted = 9,

    /// <summary>
    /// خطا در سیستم
    /// </summary>
    SystemError = 10
}