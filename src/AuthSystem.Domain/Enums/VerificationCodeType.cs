namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع کد تایید
/// این enum برای طبقه‌بندی کدهای تایید در سیستم استفاده می‌شود
/// </summary>
public enum VerificationCodeType
{
    /// <summary>
    /// کد تایید ایمیل (برای تأیید آدرس ایمیل)
    /// </summary>
    EmailVerification = 1,

    /// <summary>
    /// کد تایید شماره تلفن (برای تأیید شماره تلفن)
    /// </summary>
    PhoneVerification = 2,

    /// <summary>
    /// کد احراز هویت دو عاملی (برای تأیید هویت دو مرحله‌ای)
    /// </summary>
    TwoFactorAuth = 3,

    /// <summary>
    /// کد بازیابی رمز عبور (برای تغییر رمز عبور)
    /// </summary>
    PasswordReset = 4,

    /// <summary>
    /// کد فعال‌سازی حساب (برای فعال‌سازی حساب کاربری)
    /// </summary>
    AccountActivation = 5,

    /// <summary>
    /// کد تایید تراکنش (برای تأیید عملیات حساس)
    /// </summary>
    Transaction = 6,

    /// <summary>
    /// کد تایید دستگاه جدید (برای تأیید دستگاه جدید)
    /// </summary>
    NewDevice = 7,

    /// <summary>
    /// کد تایید تغییر ایمیل (برای تأیید تغییر آدرس ایمیل)
    /// </summary>
    EmailChange = 8,

    /// <summary>
    /// کد تایید تغییر شماره تلفن (برای تأیید تغییر شماره تلفن)
    /// </summary>
    PhoneChange = 9
}