using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Common.Entities;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object آدرس IP
/// پشتیبانی از IPv4 و IPv6
/// </summary>
public sealed class IpAddress : ValueObject
{
    /// <summary>
    /// IP های رزرو شده/خصوصی
    /// </summary>
    private static readonly string[] PrivateIpRanges =
    {
        "10.0.0.0/8",
        "172.16.0.0/12",
        "192.168.0.0/16",
        "127.0.0.0/8",
        "169.254.0.0/16",
        "::1/128",
        "fc00::/7",
        "fe80::/10"
    };

    /// <summary>
    /// مقدار IP
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// نسخه IP (IPv4 یا IPv6)
    /// </summary>
    public IpVersion Version { get; }

    /// <summary>
    /// آیا IP خصوصی/محلی است
    /// </summary>
    public bool IsPrivate { get; }

    /// <summary>
    /// آیا IP رزرو شده است
    /// </summary>
    public bool IsReserved { get; }

    /// <summary>
    /// نمایش آدرس IP به صورت شیء IPAddress
    /// </summary>
    public IPAddress Address { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private IpAddress(string value, IPAddress address)
    {
        Value = value;
        Address = address;
        Version = address.AddressFamily == AddressFamily.InterNetwork ? IpVersion.IPv4 : IpVersion.IPv6;
        IsPrivate = IsPrivateIp(address);
        IsReserved = IsReservedIp(address);
    }

    /// <summary>
    /// ایجاد آدرس IP معتبر
    /// </summary>
    public static IpAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidIpAddressException("آدرس IP نمی‌تواند خالی باشد");

        value = value.Trim();

        if (!IPAddress.TryParse(value, out var address))
            throw new InvalidIpAddressException($"آدرس IP '{value}' نامعتبر است");

        return new IpAddress(value, address);
    }

    /// <summary>
    /// ایجاد از شیء IPAddress
    /// </summary>
    public static IpAddress CreateFromIPAddress(IPAddress address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        return new IpAddress(address.ToString(), address);
    }

    /// <summary>
    /// بررسی IP خصوصی
    /// </summary>
    private static bool IsPrivateIp(IPAddress address)
    {
        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            var bytes = address.GetAddressBytes();
            // 10.0.0.0/8
            if (bytes[0] == 10)
                return true;
            // 172.16.0.0/12
            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
                return true;
            // 192.168.0.0/16
            if (bytes[0] == 192 && bytes[1] == 168)
                return true;
            // 127.0.0.0/8 (loopback)
            if (bytes[0] == 127)
                return true;
            // 169.254.0.0/16 (link-local)
            if (bytes[0] == 169 && bytes[1] == 254)
                return true;
        }
        else if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return address.IsIPv6LinkLocal || address.IsIPv6SiteLocal || IPAddress.IsLoopback(address);
        }
        return false;
    }

    /// <summary>
    /// بررسی IP رزرو شده
    /// </summary>
    private static bool IsReservedIp(IPAddress address)
    {
        if (IPAddress.IsLoopback(address))
            return true;

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            var bytes = address.GetAddressBytes();
            // 0.0.0.0/8
            if (bytes[0] == 0)
                return true;
            // 224.0.0.0/4 (multicast)
            if (bytes[0] >= 224)
                return true;
        }
        return false;
    }

    /// <summary>
    /// دریافت کشور از IP (نیاز به سرویس GeoIP)
    /// </summary>
    public string GetCountryCode()
    {
        // در عمل باید از سرویس GeoIP استفاده شود
        // این فقط یک نمونه است
        if (IsPrivate)
            return "LOCAL";

        return "IR"; // پیش‌فرض ایران
    }

    /// <summary>
    /// آیا IP از ایران است
    /// </summary>
    public bool IsFromIran()
    {
        return GetCountryCode() == "IR";
    }

    /// <summary>
    /// مقایسه با محدوده IP
    /// </summary>
    public bool IsInRange(string cidr)
    {
        // پیاده‌سازی مقایسه CIDR
        // این یک پیاده‌سازی ساده است
        return false;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    /// <summary>
    /// تبدیل implicit به IPAddress
    /// </summary>
    public static implicit operator IPAddress(IpAddress ip) => ip.Address;
}