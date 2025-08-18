// File: AuthSystem.Domain/Enums/DeliveryMethod.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// روش‌های تحویل کد تأیید
/// - این enum برای تعیین روش ارسال کد تأیید استفاده می‌شود
/// </summary>
public enum DeliveryMethod
{
    /// <summary>
    /// ایمیل
    /// </summary>
    Email = 1,

    /// <summary>
    /// پیامک
    /// </summary>
    Sms = 2,

    /// <summary>
    /// تماس تلفنی
    /// </summary>
    VoiceCall = 3,

    /// <summary>
    /// اپلیکیشن‌های پیام‌رسان
    /// </summary>
    MessagingApp = 4,

    /// <summary>
    /// اپلیکیشن احراز هویت (مثل Google Authenticator)
    /// </summary>
    AuthenticatorApp = 5,

    /// <summary>
    /// ایمیل و پیامک (ارسال همزمان)
    /// </summary>
    EmailAndSms = 6
}