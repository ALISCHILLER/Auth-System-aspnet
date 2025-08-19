using AuthSystem.Domain.Common.Exceptions;
using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای تجاوز از محدودیت نرخ درخواست‌ها
/// این استثنا زمانی رخ می‌دهد که کاربر بیش از حد مجاز درخواست ارسال کند
/// </summary>
public class RateLimitExceededException : DomainException
{
    /// <summary>
    /// کلید محدودیت نرخ (معمولاً آدرس IP یا شناسه کاربر)
    /// </summary>
    public string RateLimitKey { get; }

    /// <summary>
    /// نوع محدودیت
    /// </summary>
    public string RateLimitType { get; }

    /// <summary>
    /// زمان بازنشانی محدودیت
    /// </summary>
    public DateTime ResetTime { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "RateLimitExceeded";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public RateLimitExceededException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public RateLimitExceededException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با کلید محدودیت، نوع محدودیت، زمان بازنشانی و پیام خطا
    /// </summary>
    public RateLimitExceededException(string rateLimitKey, string rateLimitType, DateTime resetTime, string message)
        : this(message)
    {
        RateLimitKey = rateLimitKey;
        RateLimitType = rateLimitType;
        ResetTime = resetTime;
    }

    /// <summary>
    /// ایجاد استثنا برای تجاوز از محدودیت درخواست‌های ورود
    /// </summary>
    public static RateLimitExceededException ForLoginAttempts(string ip, DateTime resetTime)
    {
        var waitTime = (int)Math.Ceiling((resetTime - DateTime.UtcNow).TotalSeconds);
        return new RateLimitExceededException(
            ip,
            "Login",
            resetTime,
            $"تعداد تلاش‌های ورود بیش از حد مجاز است. لطفاً {waitTime} ثانیه صبر کنید");
    }

    /// <summary>
    /// ایجاد استثنا برای تجاوز از محدودیت درخواست‌های ارسال کد تایید
    /// </summary>
    public static RateLimitExceededException ForVerificationCodeRequests(string identifier, DateTime resetTime)
    {
        var waitTime = (int)Math.Ceiling((resetTime - DateTime.UtcNow).TotalMinutes);
        return new RateLimitExceededException(
            identifier,
            "VerificationCode",
            resetTime,
            $"تعداد درخواست‌های کد تایید بیش از حد مجاز است. لطفاً {waitTime} دقیقه صبر کنید");
    }

    /// <summary>
    /// ایجاد استثنا برای تجاوز از محدودیت درخواست‌های API
    /// </summary>
    public static RateLimitExceededException ForApiRequests(string ip, DateTime resetTime)
    {
        var waitTime = (int)Math.Ceiling((resetTime - DateTime.UtcNow).TotalSeconds);
        return new RateLimitExceededException(
            ip,
            "Api",
            resetTime,
            $"تعداد درخواست‌های API بیش از حد مجاز است. لطفاً {waitTime} ثانیه صبر کنید");
    }

    /// <summary>
    /// ایجاد استثنا برای تجاوز از محدودیت درخواست‌های بازیابی رمز عبور
    /// </summary>
    public static RateLimitExceededException ForPasswordResetRequests(string email, DateTime resetTime)
    {
        var waitTime = (int)Math.Ceiling((resetTime - DateTime.UtcNow).TotalMinutes);
        return new RateLimitExceededException(
            email,
            "PasswordReset",
            resetTime,
            $"تعداد درخواست‌های بازیابی رمز عبور بیش از حد مجاز است. لطفاً {waitTime} دقیقه صبر کنید");
    }
}