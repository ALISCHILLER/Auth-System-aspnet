// File: AuthSystem.Domain/Exceptions/InvalidPasswordException.cs
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Enums;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای رمز عبور نامعتبر
/// - هنگام اعتبارسنجی رمز عبور رخ می‌دهد
/// - شامل جزئیات خطا برای ارائه پیام مناسب به کاربر
/// </summary>
public class InvalidPasswordException : DomainException
{
    /// <summary>
    /// سطح امنیتی رمز عبور
    /// </summary>
    public SecurityLevel? SecurityLevel { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private InvalidPasswordException(string message, string errorCode, SecurityLevel? securityLevel = null)
        : base(message, errorCode)
    {
        SecurityLevel = securityLevel;
        if (securityLevel.HasValue)
            Data.Add("SecurityLevel", securityLevel.Value.ToString());
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای رمز عبور خالی
    /// </summary>
    public static InvalidPasswordException Empty()
        => new InvalidPasswordException(
            "رمز عبور نمی‌تواند خالی باشد",
            "PASSWORD_EMPTY");

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای طول نامعتبر رمز عبور
    /// </summary>
    public static InvalidPasswordException InvalidLength(int minLength, int maxLength)
    {
        var ex = new InvalidPasswordException(
            $"رمز عبور باید بین {minLength} و {maxLength} کاراکتر باشد",
            "PASSWORD_INVALID_LENGTH");

        ex.Data.Add("MinLength", minLength);
        ex.Data.Add("MaxLength", maxLength);
        return ex;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای عدم رعایت الزامات امنیتی
    /// </summary>
    public static InvalidPasswordException RequirementNotMet(string requirement, SecurityLevel securityLevel)
    {
        var ex = new InvalidPasswordException(
            $"رمز عبور باید شامل {requirement} باشد",
            "PASSWORD_REQUIREMENT_NOT_MET",
            securityLevel);

        ex.Data.Add("Requirement", requirement);
        return ex;
    }
}

/// <summary>
/// سطوح امنیتی مختلف رمز عبور
/// </summary>
