using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای زمانی که آدرس ایمیل تکراری است
/// </summary>
public class EmailAlreadyExistsException : DomainException
{
    /// <summary>
    /// آدرس ایمیل که تکراری است
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// سازنده با آدرس ایمیل
    /// </summary>
    /// <param name="email">آدرس ایمیل</param>
    public EmailAlreadyExistsException(string email)
        : base($"آدرس ایمیل {email} قبلاً ثبت شده است")
    {
        Email = email;
    }

    /// <summary>
    /// سازنده با آدرس ایمیل و پیام خطا
    /// </summary>
    /// <param name="email">آدرس ایمیل</param>
    /// <param name="message">پیام خطا</param>
    public EmailAlreadyExistsException(string email, string message)
        : base(message)
    {
        Email = email;
    }
}