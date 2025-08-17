using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای آدرس IP نامعتبر
/// </summary>
[Serializable]
public sealed class InvalidIpAddressException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.IP.INVALID";

    /// <summary>
    /// آدرس IP نامعتبر
    /// </summary>
    public string? InvalidIpAddress { get; }

    /// <summary>
    /// نوع IP مورد انتظار
    /// </summary>
    public string? ExpectedType { get; }

    public InvalidIpAddressException(string ipAddress, string? expectedType = null)
        : base(
            expectedType != null
                ? $"آدرس IP '{ipAddress}' نامعتبر است. نوع مورد انتظار: {expectedType}"
                : $"آدرس IP '{ipAddress}' نامعتبر است.",
            DEFAULT_ERROR_CODE)
    {
        InvalidIpAddress = ipAddress;
        ExpectedType = expectedType;

        WithDetail(nameof(InvalidIpAddress), ipAddress);
        if (expectedType != null)
            WithDetail(nameof(ExpectedType), expectedType);
    }

    /// <summary>
    /// ایجاد استثنا برای IP مسدود شده
    /// </summary>
    public static InvalidIpAddressException ForBlockedIp(string ipAddress)
    {
        return new InvalidIpAddressException(
            ipAddress,
            "آدرس IP مسدود شده است"
        ).WithDetail("Blocked", true) as InvalidIpAddressException;
    }
}
