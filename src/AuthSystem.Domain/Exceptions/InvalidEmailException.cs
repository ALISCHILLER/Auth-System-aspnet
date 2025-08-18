// File: AuthSystem.Domain/Exceptions/InvalidEmailException.cs
using AuthSystem.Domain.Common.Exceptions;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای آدرس ایمیل نامعتبر
/// - هنگام اعتبارسنجی ایمیل رخ می‌دهد
/// - شامل جزئیات خطا برای ارائه پیام مناسب به کاربر
/// </summary>
public class InvalidEmailException : DomainException
{
    /// <summary>
    /// آدرس ایمیل نامعتبر
    /// </summary>
    public string? Email { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private InvalidEmailException(string message, string errorCode, string? email = null)
        : base(message, errorCode)
    {
        Email = email;
        if (email != null)
            Data.Add("Email", email);
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای ایمیل خالی
    /// </summary>
    public static InvalidEmailException Empty()
        => new InvalidEmailException(
            "آدرس ایمیل نمی‌تواند خالی باشد",
            "EMAIL_EMPTY");

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای طول بیش از حد ایمیل
    /// </summary>
    public static InvalidEmailException TooLong(string email, int maxLength)
    {
        var ex = new InvalidEmailException(
            $"طول آدرس ایمیل نمی‌تواند بیشتر از {maxLength} کاراکتر باشد",
            "EMAIL_TOO_LONG",
            email);

        ex.Data.Add("MaxLength", maxLength);
        return ex;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای فرمت نامعتبر ایمیل
    /// </summary>
    public static InvalidEmailException InvalidFormat(string email, string reason)
    {
        var ex = new InvalidEmailException(
            $"فرمت آدرس ایمیل '{email}' نامعتبر است: {reason}",
            "EMAIL_INVALID_FORMAT",
            email);

        ex.Data.Add("Reason", reason);
        return ex;
    }
}