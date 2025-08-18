// File: AuthSystem.Domain/Exceptions/InvalidPhoneNumberException.cs
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Enums;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای شماره تلفن نامعتبر
/// - هنگام اعتبارسنجی شماره تلفن رخ می‌دهد
/// - شامل جزئیات خطا برای ارائه پیام مناسب به کاربر
/// </summary>
public class InvalidPhoneNumberException : DomainException
{
    /// <summary>
    /// شماره تلفن نامعتبر
    /// </summary>
    public string? PhoneNumber { get; }

    /// <summary>
    /// نوع شماره تلفن (موبایل یا ثابت)
    /// </summary>
    public PhoneType? PhoneType { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private InvalidPhoneNumberException(string message, string errorCode, string? phoneNumber = null, PhoneType? phoneType = null)
        : base(message, errorCode)
    {
        PhoneNumber = phoneNumber;
        PhoneType = phoneType;

        if (phoneNumber != null)
            Data.Add("PhoneNumber", phoneNumber);
        if (phoneType.HasValue)
            Data.Add("PhoneType", phoneType.Value.ToString());
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای شماره تلفن خالی
    /// </summary>
    public static InvalidPhoneNumberException Empty()
        => new InvalidPhoneNumberException(
            "شماره تلفن نمی‌تواند خالی باشد",
            "PHONE_NUMBER_EMPTY");

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای فرمت نامعتبر شماره تلفن
    /// </summary>
    public static InvalidPhoneNumberException InvalidFormat(string phoneNumber, string reason)
    {
        var ex = new InvalidPhoneNumberException(
            $"شماره تلفن '{phoneNumber}' نامعتبر است: {reason}",
            "PHONE_NUMBER_INVALID_FORMAT",
            phoneNumber);

        ex.Data.Add("Reason", reason);
        return ex;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای شماره تلفن غیرمجاز
    /// </summary>
    public static InvalidPhoneNumberException NotAllowed(string phoneNumber, PhoneType phoneType)
        => new InvalidPhoneNumberException(
            $"شماره تلفن '{phoneNumber}' معتبر نیست",
            "PHONE_NUMBER_NOT_ALLOWED",
            phoneNumber,
            phoneType);
}

