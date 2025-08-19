using AuthSystem.Domain.Common.Exceptions;
using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای آدرس ایمیل نامعتبر
/// این استثنا زمانی رخ می‌دهد که فرمت آدرس ایمیل صحیح نباشد
/// </summary>
public class InvalidEmailException : DomainException
{
    /// <summary>
    /// آدرس ایمیل نامعتبر
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidEmail";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidEmailException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidEmailException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با آدرس ایمیل و پیام خطا
    /// </summary>
    public InvalidEmailException(string email, string message)
        : this(message)
    {
        Email = email;
    }

    /// <summary>
    /// ایجاد استثنا برای آدرس ایمیل خالی
    /// </summary>
    public static InvalidEmailException ForEmptyEmail()
    {
        return new InvalidEmailException("آدرس ایمیل نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای آدرس ایمیل طولانی
    /// </summary>
    public static InvalidEmailException ForLongEmail(int maxLength)
    {
        return new InvalidEmailException($"آدرس ایمیل نمی‌تواند بیشتر از {maxLength} کاراکتر باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت آدرس ایمیل نامعتبر
    /// </summary>
    public static InvalidEmailException ForInvalidFormat(string email)
    {
        return new InvalidEmailException(email, $"فرمت آدرس ایمیل '{email}' نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای آدرس ایمیل موقت
    /// </summary>
    public static InvalidEmailException ForDisposableEmail(string email)
    {
        return new InvalidEmailException(email, $"آدرس ایمیل '{email}' از نوع موقت است و مجاز نمی‌باشد");
    }
}