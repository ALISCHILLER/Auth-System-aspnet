// File: AuthSystem.Domain/Enums/VerificationCodeType.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع کد تایید
/// - این enum برای تمایز بین انواع کدهای تأیید استفاده می‌شود
/// </summary>
public enum VerificationCodeType
{
    /// <summary>
    /// تایید ایمیل
    /// </summary>
    EmailVerification = 1,

    /// <summary>
    /// تایید شماره تلفن
    /// </summary>
    PhoneVerification = 2,

    /// <summary>
    /// احراز هویت دو عاملی
    /// </summary>
    TwoFactorAuth = 3,

    /// <summary>
    /// بازیابی رمز عبور
    /// </summary>
    PasswordReset = 4,

    /// <summary>
    /// فعال‌سازی حساب
    /// </summary>
    AccountActivation = 5,

    /// <summary>
    /// تایید تراکنش
    /// </summary>
    Transaction = 6,

    /// <summary>
    /// تایید تغییر ایمیل
    /// </summary>
    EmailChange = 7,

    /// <summary>
    /// تایید تغییر شماره تلفن
    /// </summary>
    PhoneChange = 8
}