using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Enums.AuthSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Exceptions;



/// <summary>
/// متدهای کمکی برای VerificationCodeType
/// </summary>
public static class VerificationCodeTypeExtensions
{
    /// <summary>
    /// دریافت طول کد تأیید بر اساس نوع
    /// </summary>
    public static int GetCodeLength(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.TwoFactorAuth => 6,           // کد 6 رقمی برای 2FA
            VerificationCodeType.Transaction => 8,              // کد 8 رقمی برای تراکنش‌ها
            VerificationCodeType.PasswordReset => 6,            // کد 6 رقمی
            VerificationCodeType.EmailVerification => 6,        // کد 6 رقمی
            VerificationCodeType.PhoneVerification => 5,        // کد 5 رقمی برای SMS
            VerificationCodeType.AccountActivation => 6,        // کد 6 رقمی
            VerificationCodeType.EmailChange => 6,              // کد 6 رقمی
            VerificationCodeType.PhoneChange => 5,              // کد 5 رقمی
            VerificationCodeType.NewDeviceLogin => 6,           // کد 6 رقمی
            VerificationCodeType.AccountDeletion => 8,          // کد 8 رقمی برای امنیت بیشتر
            VerificationCodeType.APIAccess => 10,               // کد 10 رقمی برای API
            VerificationCodeType.EmergencyVerification => 12,   // کد 12 رقمی برای اضطراری
            VerificationCodeType.MagicLink => 32,               // توکن 32 کاراکتری
            VerificationCodeType.AdminOperation => 8,           // کد 8 رقمی
            _ => 6                                              // پیش‌فرض 6 رقمی
        };
    }

    /// <summary>
    /// دریافت مدت اعتبار کد (دقیقه)
    /// </summary>
    public static int GetExpirationMinutes(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.TwoFactorAuth => 5,            // 5 دقیقه
            VerificationCodeType.Transaction => 3,               // 3 دقیقه برای امنیت
            VerificationCodeType.PasswordReset => 15,            // 15 دقیقه
            VerificationCodeType.EmailVerification => 1440,      // 24 ساعت
            VerificationCodeType.PhoneVerification => 10,        // 10 دقیقه
            VerificationCodeType.AccountActivation => 2880,      // 48 ساعت
            VerificationCodeType.EmailChange => 30,              // 30 دقیقه
            VerificationCodeType.PhoneChange => 10,              // 10 دقیقه
            VerificationCodeType.NewDeviceLogin => 10,           // 10 دقیقه
            VerificationCodeType.AccountDeletion => 60,          // 1 ساعت
            VerificationCodeType.APIAccess => 30,                // 30 دقیقه
            VerificationCodeType.EmergencyVerification => 15,    // 15 دقیقه
            VerificationCodeType.MagicLink => 15,                // 15 دقیقه
            VerificationCodeType.AdminOperation => 5,            // 5 دقیقه
            _ => 10                                              // پیش‌فرض 10 دقیقه
        };
    }

    /// <summary>
    /// بررسی نیاز به ارسال از طریق کانال امن
    /// </summary>
    public static bool RequiresSecureChannel(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.Transaction => true,
            VerificationCodeType.AccountDeletion => true,
            VerificationCodeType.APIAccess => true,
            VerificationCodeType.AdminOperation => true,
            VerificationCodeType.PasswordReset => true,
            VerificationCodeType.EmergencyVerification => true,
            _ => false
        };
    }

    /// <summary>
    /// دریافت حداکثر تعداد تلاش مجاز
    /// </summary>
    public static int GetMaxAttempts(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.TwoFactorAuth => 3,
            VerificationCodeType.Transaction => 3,
            VerificationCodeType.PasswordReset => 5,
            VerificationCodeType.AccountDeletion => 3,
            VerificationCodeType.APIAccess => 3,
            VerificationCodeType.AdminOperation => 3,
            VerificationCodeType.EmergencyVerification => 5,
            _ => 5
        };
    }

    /// <summary>
    /// بررسی نیاز به لاگ کردن
    /// </summary>
    public static bool RequiresLogging(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.Transaction => true,
            VerificationCodeType.AccountDeletion => true,
            VerificationCodeType.APIAccess => true,
            VerificationCodeType.AdminOperation => true,
            VerificationCodeType.PasswordReset => true,
            VerificationCodeType.EmergencyVerification => true,
            VerificationCodeType.TwoFactorAuth => true,
            _ => false
        };
    }

    /// <summary>
    /// دریافت نوع کد (عددی یا الفبایی)
    /// </summary>
    public static CodeFormat GetCodeFormat(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.MagicLink => CodeFormat.AlphaNumeric,
            VerificationCodeType.APIAccess => CodeFormat.AlphaNumeric,
            _ => CodeFormat.Numeric
        };
    }

    /// <summary>
    /// بررسی قابلیت ارسال مجدد
    /// </summary>
    public static bool CanResend(this VerificationCodeType type)
    {
        return type != VerificationCodeType.Transaction &&
               type != VerificationCodeType.AdminOperation;
    }

    /// <summary>
    /// دریافت حداقل فاصله بین ارسال‌ها (ثانیه)
    /// </summary>
    public static int GetResendDelaySeconds(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.TwoFactorAuth => 30,
            VerificationCodeType.PhoneVerification => 60,
            VerificationCodeType.PhoneChange => 60,
            VerificationCodeType.EmailVerification => 120,
            VerificationCodeType.EmailChange => 120,
            _ => 60
        };
    }

    /// <summary>
    /// دریافت روش ارسال پیشنهادی
    /// </summary>
    public static DeliveryMethod GetDeliveryMethod(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.EmailVerification => DeliveryMethod.Email,
            VerificationCodeType.EmailChange => DeliveryMethod.Email,
            VerificationCodeType.PhoneVerification => DeliveryMethod.SMS,
            VerificationCodeType.PhoneChange => DeliveryMethod.SMS,
            VerificationCodeType.TwoFactorAuth => DeliveryMethod.SMS | DeliveryMethod.Email | DeliveryMethod.App,
            VerificationCodeType.MagicLink => DeliveryMethod.Email,
            _ => DeliveryMethod.Email | DeliveryMethod.SMS
        };
    }

    /// <summary>
    /// دریافت متن پیام برای کاربر
    /// </summary>
    public static string GetUserMessage(this VerificationCodeType type, string code)
    {
        return type switch
        {
            VerificationCodeType.EmailVerification => $"کد تأیید ایمیل شما: {code}",
            VerificationCodeType.PhoneVerification => $"کد تأیید موبایل: {code}",
            VerificationCodeType.TwoFactorAuth => $"کد ورود دو مرحله‌ای: {code}",
            VerificationCodeType.PasswordReset => $"کد بازیابی رمز عبور: {code}",
            VerificationCodeType.AccountActivation => $"کد فعال‌سازی حساب: {code}",
            VerificationCodeType.Transaction => $"کد تأیید تراکنش: {code}",
            VerificationCodeType.EmailChange => $"کد تأیید ایمیل جدید: {code}",
            VerificationCodeType.PhoneChange => $"کد تأیید موبایل جدید: {code}",
            VerificationCodeType.NewDeviceLogin => $"کد تأیید دستگاه جدید: {code}",
            VerificationCodeType.AccountDeletion => $"کد تأیید حذف حساب: {code}",
            VerificationCodeType.APIAccess => $"کد دسترسی API: {code}",
            VerificationCodeType.EmergencyVerification => $"کد تأیید اضطراری: {code}",
            VerificationCodeType.MagicLink => $"لینک ورود یکبار مصرف: {code}",
            VerificationCodeType.AdminOperation => $"کد تأیید عملیات مدیریتی: {code}",
            _ => $"کد تأیید: {code}"
        };
    }

    /// <summary>
    /// بررسی حساسیت امنیتی
    /// </summary>
    public static SecurityLevel GetSecurityLevel(this VerificationCodeType type)
    {
        return type switch
        {
            VerificationCodeType.Transaction => SecurityLevel.Critical,
            VerificationCodeType.AccountDeletion => SecurityLevel.Critical,
            VerificationCodeType.APIAccess => SecurityLevel.High,
            VerificationCodeType.AdminOperation => SecurityLevel.Critical,
            VerificationCodeType.PasswordReset => SecurityLevel.High,
            VerificationCodeType.EmergencyVerification => SecurityLevel.High,
            VerificationCodeType.TwoFactorAuth => SecurityLevel.High,
            VerificationCodeType.EmailChange => SecurityLevel.Medium,
            VerificationCodeType.PhoneChange => SecurityLevel.Medium,
            VerificationCodeType.NewDeviceLogin => SecurityLevel.Medium,
            _ => SecurityLevel.Low
        };
    }
}