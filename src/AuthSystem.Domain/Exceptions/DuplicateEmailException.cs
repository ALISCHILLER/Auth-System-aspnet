// File: AuthSystem.Domain/Exceptions/DuplicateEmailException.cs
using AuthSystem.Domain.Common.Exceptions;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای تکراری بودن آدرس ایمیل
/// - هنگام ثبت‌نام یا تغییر ایمیل رخ می‌دهد
/// - نشان‌دهنده این است که ایمیل مورد نظر قبلاً توسط کاربر دیگری استفاده شده است
/// </summary>
public class DuplicateEmailException : DomainException
{
    /// <summary>
    /// آدرس ایمیل تکراری
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private DuplicateEmailException(string email, string message, string errorCode)
        : base(message, errorCode)
    {
        Email = email;
        Data.Add("Email", email);
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا با آدرس ایمیل تکراری
    /// </summary>
    public static DuplicateEmailException ForEmail(string email)
        => new DuplicateEmailException(
            email,
            $"آدرس ایمیل '{email}' قبلاً توسط کاربر دیگری استفاده شده است",
            "EMAIL_ALREADY_EXISTS");

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا با آدرس ایمیل تکراری و استثنای داخلی
    /// </summary>
    public static DuplicateEmailException ForEmailWithInnerException(string email, Exception innerException)
        => new DuplicateEmailException(
            email,
            $"آدرس ایمیل '{email}' قبلاً توسط کاربر دیگری استفاده شده است",
            "EMAIL_ALREADY_EXISTS")
        {
            InnerException = innerException
        };

    /// <summary>
    /// سازنده استاتیک برای استفاده در موارد عمومی
    /// </summary>
    public static DuplicateEmailException General()
        => new DuplicateEmailException(
            null,
            "آدرس ایمیل قبلاً توسط کاربر دیگری استفاده شده است",
            "EMAIL_ALREADY_EXISTS");
}