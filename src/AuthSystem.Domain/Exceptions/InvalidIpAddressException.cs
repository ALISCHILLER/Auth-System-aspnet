using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای آدرس IP نامعتبر
/// این استثنا زمانی رخ می‌دهد که فرمت آدرس IP صحیح نباشد
/// </summary>
public class InvalidIpAddressException : DomainException
{
    /// <summary>
    /// آدرس IP نامعتبر
    /// </summary>
    public string IpAddress { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidIpAddress";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidIpAddressException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidIpAddressException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با آدرس IP و پیام خطا
    /// </summary>
    public InvalidIpAddressException(string ipAddress, string message)
        : this(message)
    {
        IpAddress = ipAddress;
    }

    /// <summary>
    /// ایجاد استثنا برای آدرس IP خالی
    /// </summary>
    public static InvalidIpAddressException ForEmptyIpAddress()
    {
        return new InvalidIpAddressException("آدرس IP نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت آدرس IP نامعتبر
    /// </summary>
    public static InvalidIpAddressException ForInvalidFormat(string ipAddress)
    {
        return new InvalidIpAddressException(ipAddress, $"فرمت آدرس IP '{ipAddress}' نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای آدرس IP رزرو شده
    /// </summary>
    public static InvalidIpAddressException ForReservedIpAddress(string ipAddress)
    {
        return new InvalidIpAddressException(ipAddress, $"آدرس IP '{ipAddress}' رزرو شده است و نمی‌تواند استفاده شود");
    }

    /// <summary>
    /// ایجاد استثنا برای آدرس IP ناشناخته
    /// </summary>
    public static InvalidIpAddressException ForUnknownIpAddress(string ipAddress)
    {
        return new InvalidIpAddressException(ipAddress, $"آدرس IP '{ipAddress}' ناشناخته است");
    }
}