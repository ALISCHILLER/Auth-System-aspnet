// AuthSystem.Domain/Enums/TokenType.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع توکن در سیستم احراز هویت
/// </summary>
public enum TokenType
{
    Access,          // توکن دسترسی
    Refresh,         // توکن نوسازی
    EmailVerification,    // توکن تایید ایمیل
    PasswordReset,   // توکن بازیابی رمز عبور
    TwoFactor,       // توکن احراز هویت دو عاملی
    ApiKey,          // کلید API
    Session          // توکن جلسه
}