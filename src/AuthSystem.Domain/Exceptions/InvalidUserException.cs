using AuthSystem.Domain.Common.Exceptions;
using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای کاربر نامعتبر
/// این استثنا زمانی رخ می‌دهد که کاربر از قوانین سیستم پیروی نکند
/// </summary>
public class InvalidUserException : DomainException
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid? UserId { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidUser";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidUserException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidUserException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با شناسه کاربر و پیام خطا
    /// </summary>
    public InvalidUserException(Guid userId, string message)
        : this(message)
    {
        UserId = userId;
    }

    /// <summary>
    /// ایجاد استثنا برای کاربر فاقد اطلاعات ضروری
    /// </summary>
    public static InvalidUserException ForMissingRequiredData()
    {
        return new InvalidUserException("کاربر فاقد اطلاعات ضروری است");
    }

    /// <summary>
    /// ایجاد استثنا برای کاربر با ایمیل نامعتبر
    /// </summary>
    public static InvalidUserException ForInvalidEmail(string email)
    {
        return new InvalidUserException($"ایمیل '{email}' نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کاربر با رمز عبور نامعتبر
    /// </summary>
    public static InvalidUserException ForInvalidPassword()
    {
        return new InvalidUserException("رمز عبور نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کاربر با شماره تلفن نامعتبر
    /// </summary>
    public static InvalidUserException ForInvalidPhoneNumber(string phoneNumber)
    {
        return new InvalidUserException($"شماره تلفن '{phoneNumber}' نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کاربر با کد ملی نامعتبر
    /// </summary>
    public static InvalidUserException ForInvalidNationalCode(string nationalCode)
    {
        return new InvalidUserException($"کد ملی '{nationalCode}' نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کاربر فاقد نقش
    /// </summary>
    public static InvalidUserException ForMissingRole()
    {
        return new InvalidUserException("کاربر باید حداقل یک نقش داشته باشد");
    }
}