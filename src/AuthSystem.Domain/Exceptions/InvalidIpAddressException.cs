// File: AuthSystem.Domain/Exceptions/InvalidIpAddressException.cs
using AuthSystem.Domain.Common.Exceptions;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای آدرس IP نامعتبر
/// - هنگام اعتبارسنجی آدرس IP رخ می‌دهد
/// - شامل جزئیات خطا برای ارائه پیام مناسب به کاربر
/// </summary>
public class InvalidIpAddressException : DomainException
{
    /// <summary>
    /// آدرس IP نامعتبر
    /// </summary>
    public string? IpAddress { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private InvalidIpAddressException(string message, string errorCode, string? ipAddress = null)
        : base(message, errorCode)
    {
        IpAddress = ipAddress;
        if (ipAddress != null)
            Data.Add("IpAddress", ipAddress);
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای آدرس IP خالی
    /// </summary>
    public static InvalidIpAddressException Empty()
        => new InvalidIpAddressException(
            "آدرس IP نمی‌تواند خالی باشد",
            "IP_ADDRESS_EMPTY");

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای فرمت نامعتبر آدرس IP
    /// </summary>
    public static InvalidIpAddressException InvalidFormat(string ipAddress)
        => new InvalidIpAddressException(
            $"فرمت آدرس IP '{ipAddress}' نامعتبر است",
            "IP_ADDRESS_INVALID_FORMAT",
            ipAddress);

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای آدرس IP غیرمجاز
    /// </summary>
    public static InvalidIpAddressException NotAllowed(string ipAddress, string reason)
    {
        var ex = new InvalidIpAddressException(
            $"آدرس IP '{ipAddress}' غیرمجاز است: {reason}",
            "IP_ADDRESS_NOT_ALLOWED",
            ipAddress);

        ex.Data.Add("Reason", reason);
        return ex;
    }
}