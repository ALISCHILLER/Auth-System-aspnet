using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای کلید محرمانه احراز هویت دو عاملی نامعتبر
/// این استثنا زمانی رخ می‌دهد که کلید محرمانه 2FA از قوانین سیستم پیروی نکند
/// </summary>
public class InvalidTwoFactorSecretKeyException : DomainException
{
    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidTwoFactorSecretKey";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidTwoFactorSecretKeyException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidTwoFactorSecretKeyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// ایجاد استثنا برای کلید محرمانه خالی
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForEmptySecretKey()
    {
        return new InvalidTwoFactorSecretKeyException("کلید محرمانه نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت کلید محرمانه نامعتبر
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForInvalidFormat(string reason)
    {
        return new InvalidTwoFactorSecretKeyException($"فرمت کلید محرمانه نامعتبر است: {reason}");
    }

    /// <summary>
    /// ایجاد استثنا برای کلید محرمانه غیرفعال
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForInactiveSecretKey()
    {
        return new InvalidTwoFactorSecretKeyException("کلید محرمانه غیرفعال است");
    }

    /// <summary>
    /// ایجاد استثنا برای کلید محرمانه منقضی شده
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForExpiredSecretKey()
    {
        return new InvalidTwoFactorSecretKeyException("کلید محرمانه منقضی شده است");
    }

    /// <summary>
    /// ایجاد استثنا برای کلید محرمانه نامعتبر
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForInvalidSecretKey()
    {
        return new InvalidTwoFactorSecretKeyException("کلید محرمانه نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کلید محرمانه تکراری
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForDuplicateSecretKey()
    {
        return new InvalidTwoFactorSecretKeyException("کلید محرمانه تکراری است");
    }

    /// <summary>
    /// ایجاد استثنا برای کلید محرمانه با فرمت Base32 نامعتبر
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForInvalidBase32Format()
    {
        return new InvalidTwoFactorSecretKeyException("کلید محرمانه باید در فرمت Base32 باشد");
    }
}