// File: AuthSystem.Domain/Exceptions/InvalidNationalCodeException.cs
using AuthSystem.Domain.Common.Exceptions;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای کد ملی نامعتبر
/// - هنگام اعتبارسنجی کد ملی رخ می‌دهد
/// - شامل جزئیات خطا برای ارائه پیام مناسب به کاربر
/// </summary>
public class InvalidNationalCodeException : DomainException
{
    /// <summary>
    /// کد ملی نامعتبر
    /// </summary>
    public string? NationalCode { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private InvalidNationalCodeException(string message, string errorCode, string? nationalCode = null)
        : base(message, errorCode)
    {
        NationalCode = nationalCode;
        if (nationalCode != null)
            Data.Add("NationalCode", nationalCode);
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای کد ملی خالی
    /// </summary>
    public static InvalidNationalCodeException Empty()
        => new InvalidNationalCodeException(
            "کد ملی نمی‌تواند خالی باشد",
            "NATIONAL_CODE_EMPTY");

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای طول نامعتبر کد ملی
    /// </summary>
    public static InvalidNationalCodeException InvalidLength(string nationalCode, int requiredLength)
    {
        var ex = new InvalidNationalCodeException(
            $"کد ملی باید {requiredLength} رقم باشد",
            "NATIONAL_CODE_INVALID_LENGTH",
            nationalCode);

        ex.Data.Add("RequiredLength", requiredLength);
        return ex;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای فرمت نامعتبر کد ملی
    /// </summary>
    public static InvalidNationalCodeException InvalidFormat(string nationalCode, string reason)
    {
        var ex = new InvalidNationalCodeException(
            $"کد ملی '{nationalCode}' نامعتبر است: {reason}",
            "NATIONAL_CODE_INVALID_FORMAT",
            nationalCode);

        ex.Data.Add("Reason", reason);
        return ex;
    }
}