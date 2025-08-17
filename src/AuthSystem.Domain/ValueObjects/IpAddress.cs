using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using System.Net;
using System.Net.Sockets;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object آدرس IP با تشخیص نوع
/// </summary>
public sealed class IpAddress : ValueObject
{
    public string Value { get; }
    public AddressFamily AddressFamily { get; }
    public bool IsIPv4 => AddressFamily == AddressFamily.InterNetwork;
    public bool IsIPv6 => AddressFamily == AddressFamily.InterNetworkV6;

    private IpAddress(string value, AddressFamily addressFamily)
    {
        Value = value;
        AddressFamily = addressFamily;
    }

    public static IpAddress Create(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new InvalidIpAddressException("آدرس IP نمی‌تواند خالی باشد");

        if (!IPAddress.TryParse(ipAddress, out var parsedIp))
            throw new InvalidIpAddressException($"آدرس IP '{ipAddress}' نامعتبر است");

        return new IpAddress(parsedIp.ToString(), parsedIp.AddressFamily);
    }

    public bool IsPrivate()
    {
        var ip = IPAddress.Parse(Value);
        if (IsIPv4)
        {
            var bytes = ip.GetAddressBytes();
            return (bytes[0] == 10) ||
                   (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
                   (bytes[0] == 192 && bytes[1] == 168);
        }
        if (IsIPv6)
        {
            var addressString = ip.ToString();
            return addressString.StartsWith("fe80:", StringComparison.OrdinalIgnoreCase) ||
                   addressString.StartsWith("fc00:", StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public bool IsLoopback() => IPAddress.IsLoopback(IPAddress.Parse(Value));

    protected override IEnumerable<object> GetEqualityComponents() => new[] { Value };
    public override string ToString() => Value;
}