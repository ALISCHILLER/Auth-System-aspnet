using System;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// Exception thrown when rate limits are exceeded.
/// </summary>
public class RateLimitExceededException : DomainException
{
    
    public string RateLimitKey { get; }

    public string RateLimitType { get; }

   
    public DateTime ResetTime { get; }

  
    public override string ErrorCode => "RateLimitExceeded";

    public RateLimitExceededException(string message) : base(message)
    {
    }

   
    public RateLimitExceededException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    
    public RateLimitExceededException(string rateLimitKey, string rateLimitType, DateTime resetTime, string message)
        : this(message)
    {
        RateLimitKey = rateLimitKey;
        RateLimitType = rateLimitType;
        ResetTime = resetTime;
    }

    
    public static RateLimitExceededException ForLoginAttempts(string ip, DateTime resetTime)
    {
        var waitTime = CalculateSeconds(resetTime);
        return new RateLimitExceededException(
            ip,
            "Login",
            resetTime,
            $"تعداد تلاش‌های ورود بیش از حد مجاز است. لطفاً {waitTime} ثانیه صبر کنید");
    }

  
    public static RateLimitExceededException ForVerificationCodeRequests(string identifier, DateTime resetTime)
    {
        var waitTime = CalculateMinutes(resetTime);
        return new RateLimitExceededException(
            identifier,
            "VerificationCode",
            resetTime,
            $"تعداد درخواست‌های کد تایید بیش از حد مجاز است. لطفاً {waitTime} دقیقه صبر کنید");
    }

    
    public static RateLimitExceededException ForApiRequests(string ip, DateTime resetTime)
    {
        var waitTime = CalculateSeconds(resetTime);
        return new RateLimitExceededException(
            ip,
            "Api",
            resetTime,
            $"تعداد درخواست‌های API بیش از حد مجاز است. لطفاً {waitTime} ثانیه صبر کنید");
    }

  
    public static RateLimitExceededException ForPasswordResetRequests(string email, DateTime resetTime)
    {
        var waitTime = CalculateMinutes(resetTime);
        return new RateLimitExceededException(
            email,
            "PasswordReset",
            resetTime,
            $"تعداد درخواست‌های بازیابی رمز عبور بیش از حد مجاز است. لطفاً {waitTime} دقیقه صبر کنید");
    }
    private static int CalculateSeconds(DateTime resetTime)
    {
        var now = DomainClock.Instance.UtcNow;
        return (int)Math.Ceiling((resetTime - now).TotalSeconds);
    }

    private static int CalculateMinutes(DateTime resetTime)
    {
        var now = DomainClock.Instance.UtcNow;
        return (int)Math.Ceiling((resetTime - now).TotalMinutes);
    }
}