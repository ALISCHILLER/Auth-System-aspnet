using AuthSystem.Domain.Common.Exceptions;
using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای کد تایید نامعتبر
/// این استثنا زمانی رخ می‌دهد که کد تایید از قوانین سیستم پیروی نکند
/// </summary>
public class InvalidVerificationCodeException : DomainException
{
    /// <summary>
    /// نوع کد تایید
    /// </summary>
    public string CodeType { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidVerificationCode";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidVerificationCodeException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidVerificationCodeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با نوع کد و پیام خطا
    /// </summary>
    public InvalidVerificationCodeException(string codeType, string message)
        : this(message)
    {
        CodeType = codeType;
    }

    /// <summary>
    /// ایجاد استثنا برای کد تایید خالی
    /// </summary>
    public static InvalidVerificationCodeException ForEmptyCode()
    {
        return new InvalidVerificationCodeException("کد تایید نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت کد تایید نامعتبر
    /// </summary>
    public static InvalidVerificationCodeException ForInvalidFormat(string code)
    {
        return new InvalidVerificationCodeException($"فرمت کد تایید '{code}' نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کد تایید منقضی شده
    /// </summary>
    public static InvalidVerificationCodeException ForExpiredCode()
    {
        return new InvalidVerificationCodeException("کد تایید منقضی شده است");
    }

    /// <summary>
    /// ایجاد استثنا برای کد تایید استفاده شده
    /// </summary>
    public static InvalidVerificationCodeException ForAlreadyUsedCode()
    {
        return new InvalidVerificationCodeException("کد تایید قبلاً استفاده شده است");
    }

    /// <summary>
    /// ایجاد استثنا برای تعداد تلاش‌های بیش از حد مجاز
    /// </summary>
    public static InvalidVerificationCodeException ForMaxAttemptsExceeded()
    {
        return new InvalidVerificationCodeException("تعداد تلاش‌ها بیش از حد مجاز است");
    }

    /// <summary>
    /// ایجاد استثنا برای کد تایید نامرتبط با درخواست
    /// </summary>
    public static InvalidVerificationCodeException ForMismatchedCode()
    {
        return new InvalidVerificationCodeException("کد تایید با درخواست مرتبط نیست");
    }

    /// <summary>
    /// ایجاد استثنا برای کد تایید نامعتبر
    /// </summary>
    public static InvalidVerificationCodeException ForInvalidCode()
    {
        return new InvalidVerificationCodeException("کد تایید نامعتبر است");
    }
}