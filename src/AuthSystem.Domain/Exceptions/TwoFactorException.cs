using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا عمومی برای خطاهای احراز هویت دو عاملی
/// این استثنا زمانی رخ می‌دهد که خطایی در فرآیند 2FA رخ دهد
/// </summary>
public class TwoFactorException : DomainException
{
    /// <summary>
    /// نوع خطا در احراز هویت دو عاملی
    /// </summary>
    public TwoFactorErrorType ErrorType { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "TwoFactorError";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public TwoFactorException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public TwoFactorException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با نوع خطا و پیام خطا
    /// </summary>
    public TwoFactorException(TwoFactorErrorType errorType, string message)
        : this(message)
    {
        ErrorType = errorType;
    }

    /// <summary>
    /// ایجاد استثنا برای کد نامعتبر
    /// </summary>
    public static TwoFactorException ForInvalidCode()
    {
        return new TwoFactorException(TwoFactorErrorType.InvalidCode, "کد وارد شده نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کد منقضی شده
    /// </summary>
    public static TwoFactorException ForExpiredCode()
    {
        return new TwoFactorException(TwoFactorErrorType.ExpiredCode, "کد وارد شده منقضی شده است");
    }

    /// <summary>
    /// ایجاد استثنا برای تعداد تلاش‌های بیش از حد
    /// </summary>
    public static TwoFactorException ForTooManyAttempts()
    {
        return new TwoFactorException(TwoFactorErrorType.TooManyAttempts, "تعداد تلاش‌ها بیش از حد مجاز است");
    }

    /// <summary>
    /// ایجاد استثنا برای کلید محرمانه نامعتبر
    /// </summary>
    public static TwoFactorException ForInvalidSecretKey()
    {
        return new TwoFactorException(TwoFactorErrorType.InvalidSecretKey, "کلید محرمانه نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کلید محرمانه غیرفعال
    /// </summary>
    public static TwoFactorException ForInactiveSecretKey()
    {
        return new TwoFactorException(TwoFactorErrorType.InactiveSecretKey, "کلید محرمانه غیرفعال است");
    }

    /// <summary>
    /// ایجاد استثنا برای دستگاه ناشناخته
    /// </summary>
    public static TwoFactorException ForUnknownDevice()
    {
        return new TwoFactorException(TwoFactorErrorType.UnknownDevice, "دستگاه شما شناخته شده نیست");
    }

    /// <summary>
    /// ایجاد استثنا برای خطا در ارسال کد
    /// </summary>
    public static TwoFactorException ForDeliveryFailed()
    {
        return new TwoFactorException(TwoFactorErrorType.DeliveryFailed, "ارسال کد با خطا مواجه شد");
    }

    /// <summary>
    /// ایجاد استثنا برای احراز هویت دو عاملی غیرفعال
    /// </summary>
    public static TwoFactorException ForTwoFactorNotEnabled()
    {
        return new TwoFactorException(TwoFactorErrorType.TwoFactorNotEnabled, "احراز هویت دو عاملی فعال نیست");
    }
}