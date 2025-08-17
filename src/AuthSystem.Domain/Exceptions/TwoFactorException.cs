using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای خطاهای احراز هویت دو عاملی
/// </summary>
[Serializable]
public sealed class TwoFactorException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.2FA.ERROR";

    /// <summary>
    /// نوع خطای احراز هویت دو عاملی
    /// </summary>
    public TwoFactorErrorType ErrorType { get; }

    /// <summary>
    /// تعداد تلاش‌های باقی‌مانده
    /// </summary>
    public int? RemainingAttempts { get; private set; } // اضافه کردن private setter

    /// <summary>
    /// زمان باقی‌مانده تا امکان تلاش مجدد (ثانیه)
    /// </summary>
    public int? RetryAfterSeconds { get; private set; } // اضافه کردن private setter

    private TwoFactorException(
        string message,
        TwoFactorErrorType errorType = TwoFactorErrorType.GeneralError,
        string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
        ErrorType = errorType;
        WithDetail(nameof(ErrorType), errorType.ToString());
    }

    /// <summary>
    /// متد WithDetail با بازگشت نوع صحیح
    /// </summary>
    public new TwoFactorException WithDetail(string key, object value)
    {
        base.WithDetail(key, value);
        return this;
    }

    /// <summary>
    /// تنظیم تعداد تلاش‌های باقی‌مانده
    /// </summary>
    private TwoFactorException WithRemainingAttempts(int attempts)
    {
        RemainingAttempts = attempts;
        return WithDetail(nameof(RemainingAttempts), attempts);
    }

    /// <summary>
    /// تنظیم زمان انتظار
    /// </summary>
    private TwoFactorException WithRetryAfter(int seconds)
    {
        RetryAfterSeconds = seconds;
        return WithDetail(nameof(RetryAfterSeconds), seconds);
    }

    /// <summary>
    /// ایجاد استثنا برای کد نامعتبر
    /// </summary>
    public static TwoFactorException ForInvalidCode(int remainingAttempts)
    {
        var exception = new TwoFactorException(
            $"کد وارد شده نامعتبر است. {remainingAttempts} تلاش باقی مانده است.",
            TwoFactorErrorType.InvalidCode,
            "AUTH.2FA.INVALID_CODE"
        );
        return exception.WithRemainingAttempts(remainingAttempts);
    }

    /// <summary>
    /// ایجاد استثنا برای کد منقضی شده
    /// </summary>
    public static TwoFactorException ForExpiredCode()
    {
        return new TwoFactorException(
            "کد تأیید منقضی شده است. لطفاً کد جدید درخواست کنید.",
            TwoFactorErrorType.CodeExpired,
            "AUTH.2FA.CODE_EXPIRED"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای تلاش‌های زیاد
    /// </summary>
    public static TwoFactorException ForTooManyAttempts(int lockoutMinutes = 15)
    {
        var exception = new TwoFactorException(
            $"تعداد تلاش‌های شما بیش از حد مجاز است. لطفاً {lockoutMinutes} دقیقه صبر کنید.",
            TwoFactorErrorType.TooManyAttempts,
            "AUTH.2FA.TOO_MANY_ATTEMPTS"
        );
        return exception
            .WithRetryAfter(lockoutMinutes * 60)
            .WithDetail("LockoutMinutes", lockoutMinutes);
    }

    /// <summary>
    /// ایجاد استثنا برای احراز هویت دو عاملی غیرفعال
    /// </summary>
    public static TwoFactorException ForNotEnabled()
    {
        return new TwoFactorException(
            "احراز هویت دو عاملی برای این حساب فعال نیست.",
            TwoFactorErrorType.NotEnabled,
            "AUTH.2FA.NOT_ENABLED"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای کد قبلاً استفاده شده
    /// </summary>
    public static TwoFactorException ForAlreadyUsedCode()
    {
        return new TwoFactorException(
            "این کد قبلاً استفاده شده است.",
            TwoFactorErrorType.CodeAlreadyUsed,
            "AUTH.2FA.CODE_ALREADY_USED"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای روش تأیید نامعتبر
    /// </summary>
    public static TwoFactorException ForInvalidMethod(string method)
    {
        var exception = new TwoFactorException(
            $"روش احراز هویت '{method}' پشتیبانی نمی‌شود.",
            TwoFactorErrorType.InvalidMethod,
            "AUTH.2FA.INVALID_METHOD"
        );
        return exception.WithDetail("Method", method);
    }

    /// <summary>
    /// ایجاد استثنا برای خطای تولید کد
    /// </summary>
    public static TwoFactorException ForGenerationError()
    {
        return new TwoFactorException(
            "خطا در تولید کد احراز هویت. لطفاً دوباره تلاش کنید.",
            TwoFactorErrorType.GenerationError,
            "AUTH.2FA.GENERATION_ERROR"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای درخواست مکرر
    /// </summary>
    public static TwoFactorException ForRateLimitExceeded(int waitSeconds)
    {
        var exception = new TwoFactorException(
            $"درخواست شما بیش از حد مجاز است. لطفاً {waitSeconds} ثانیه صبر کنید.",
            TwoFactorErrorType.RateLimitExceeded,
            "AUTH.2FA.RATE_LIMIT"
        );
        return exception
            .WithRetryAfter(waitSeconds)
            .WithDetail("WaitSeconds", waitSeconds);
    }
}

/// <summary>
/// انواع خطاهای احراز هویت دو عاملی
/// </summary>
public enum TwoFactorErrorType
{
    GeneralError,
    InvalidCode,
    CodeExpired,
    TooManyAttempts,
    NotEnabled,
    CodeAlreadyUsed,
    InvalidMethod,
    GenerationError,
    RateLimitExceeded
}
