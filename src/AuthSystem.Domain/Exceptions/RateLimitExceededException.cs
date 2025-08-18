// File: AuthSystem.Domain/Exceptions/RateLimitExceededException.cs
using AuthSystem.Domain.Common.Exceptions;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای تجاوز از محدودیت نرخ درخواست‌ها
/// - هنگام تجاوز از تعداد مجاز درخواست در بازه زمانی مشخص رخ می‌دهد
/// - شامل جزئیات محدودیت و زمان باقی‌مانده تا بازنشانی
/// </summary>
public class RateLimitExceededException : DomainException
{
    /// <summary>
    /// کلید مربوط به محدودیت نرخ
    /// </summary>
    public string RateLimitKey { get; }

    /// <summary>
    /// حداکثر تعداد درخواست مجاز
    /// </summary>
    public int MaxRequests { get; }

    /// <summary>
    /// بازه زمانی محدودیت (ثانیه)
    /// </summary>
    public int WindowSeconds { get; }

    /// <summary>
    /// زمان بازنشانی محدودیت (UTC)
    /// </summary>
    public DateTime ResetTime { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private RateLimitExceededException(
        string message,
        string errorCode,
        string rateLimitKey,
        int maxRequests,
        int windowSeconds,
        DateTime resetTime)
        : base(message, errorCode)
    {
        RateLimitKey = rateLimitKey;
        MaxRequests = maxRequests;
        WindowSeconds = windowSeconds;
        ResetTime = resetTime;

        Data.Add("RateLimitKey", rateLimitKey);
        Data.Add("MaxRequests", maxRequests);
        Data.Add("WindowSeconds", windowSeconds);
        Data.Add("ResetTime", resetTime);
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای محدودیت تلاش‌های ورود
    /// </summary>
    public static RateLimitExceededException ForLoginAttempts(int maxAttempts, TimeSpan lockoutTime)
    {
        var resetTime = DateTime.UtcNow.Add(lockoutTime);
        return new RateLimitExceededException(
            $"تعداد درخواست‌ها بیش از حد مجاز است. لطفاً {GetTimeRemaining(resetTime)} دیگر تلاش کنید.",
            "RATE_LIMIT_EXCEEDED",
            "LOGIN_ATTEMPTS",
            maxAttempts,
            (int)lockoutTime.TotalSeconds,
            resetTime);
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای محدودیت تولید کد تایید
    /// </summary>
    public static RateLimitExceededException ForVerificationCode(int maxRequests, TimeSpan resetTime)
    {
        var resetDateTime = DateTime.UtcNow.Add(resetTime);
        return new RateLimitExceededException(
            $"تعداد درخواست‌ها بیش از حد مجاز است. لطفاً {GetTimeRemaining(resetDateTime)} دیگر تلاش کنید.",
            "RATE_LIMIT_EXCEEDED",
            "VERIFICATION_CODE",
            maxRequests,
            (int)resetTime.TotalSeconds,
            resetDateTime);
    }

    /// <summary>
    /// محاسبه زمان باقی‌مانده تا بازنشانی
    /// </summary>
    private static string GetTimeRemaining(DateTime resetTime)
    {
        var remaining = resetTime - DateTime.UtcNow;
        if (remaining.TotalMinutes >= 1)
            return $"{(int)remaining.TotalMinutes} دقیقه و {remaining.Seconds} ثانیه";
        return $"{remaining.Seconds} ثانیه";
    }
}