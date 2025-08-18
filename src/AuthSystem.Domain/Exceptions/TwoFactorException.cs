// File: AuthSystem.Domain/Exceptions/TwoFactorException.cs
using AuthSystem.Domain.Common.Exceptions;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا عمومی برای خطاهای احراز هویت دو عاملی
/// - برای خطاهای عمومی مرتبط با 2FA استفاده می‌شود
/// - شامل انواع خطاها مانند کد نامعتبر، زمان انقضای کد و غیره
/// </summary>
public class TwoFactorException : DomainException
{
    /// <summary>
    /// نوع خطا در احراز هویت دو عاملی
    /// </summary>
    public TwoFactorErrorType ErrorType { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private TwoFactorException(string message, string errorCode, TwoFactorErrorType errorType)
        : base(message, errorCode)
    {
        ErrorType = errorType;
        Data.Add("ErrorType", errorType.ToString());
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای کد تایید نامعتبر
    /// </summary>
    public static TwoFactorException InvalidCode(string code)
        => new TwoFactorException(
            $"کد تایید '{code}' نامعتبر است یا منقضی شده است",
            "TWO_FACTOR_INVALID_CODE",
            TwoFactorErrorType.InvalidCode)
        {
            Data = { ["Code"] = code }
        };

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای زمان انقضای کد
    /// </summary>
    public static TwoFactorException CodeExpired(TimeSpan validityPeriod)
        => new TwoFactorException(
            $"کد تایید منقضی شده است. کدها تنها به مدت {validityPeriod:mm\\:ss} معتبر هستند",
            "TWO_FACTOR_CODE_EXPIRED",
            TwoFactorErrorType.CodeExpired)
        {
            Data = { ["ValidityPeriod"] = validityPeriod }
        };

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای تعداد تلاش‌های بیش از حد
    /// </summary>
    public static TwoFactorException MaxAttemptsExceeded(int maxAttempts, int remainingAttempts)
    {
        var ex = new TwoFactorException(
            $"تعداد تلاش‌ها بیش از حد مجاز است. {remainingAttempts} تلاش باقی‌مانده است",
            "TWO_FACTOR_MAX_ATTEMPTS_EXCEEDED",
            TwoFactorErrorType.MaxAttemptsExceeded);

        ex.Data.Add("MaxAttempts", maxAttempts);
        ex.Data.Add("RemainingAttempts", remainingAttempts);
        return ex;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای 2FA غیرفعال
    /// </summary>
    public static TwoFactorException NotEnabled()
        => new TwoFactorException(
            "احراز هویت دو عاملی برای این حساب فعال نیست",
            "TWO_FACTOR_NOT_ENABLED",
            TwoFactorErrorType.NotEnabled);

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای نیاز به تنظیم 2FA
    /// </summary>
    public static TwoFactorException SetupRequired()
        => new TwoFactorException(
            "نیاز به تنظیم احراز هویت دو عاملی است",
            "TWO_FACTOR_SETUP_REQUIRED",
            TwoFactorErrorType.SetupRequired);
}

/// <summary>
/// انواع خطاهای احراز هویت دو عاملی
/// </summary>
public enum TwoFactorErrorType
{
    /// <summary>
    /// کد تایید نامعتبر است
    /// </summary>
    InvalidCode,

    /// <summary>
    /// کد تایید منقضی شده است
    /// </summary>
    CodeExpired,

    /// <summary>
    /// تعداد تلاش‌ها بیش از حد مجاز است
    /// </summary>
    MaxAttemptsExceeded,

    /// <summary>
    /// احراز هویت دو عاملی فعال نیست
    /// </summary>
    NotEnabled,

    /// <summary>
    /// نیاز به تنظیم احراز هویت دو عاملی است
    /// </summary>
    SetupRequired
}