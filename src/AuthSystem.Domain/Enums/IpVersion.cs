// File: AuthSystem.Domain/Enums/IpVersion.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// نسخه‌های پروتکل IP
/// - این enum برای تشخیص نسخه IP استفاده می‌شود
/// </summary>
public enum IpVersion
{
    /// <summary>
    /// نسخه 4 پروتکل IP
    /// </summary>
    IPv4 = 4,

    /// <summary>
    /// نسخه 6 پروتکل IP
    /// </summary>
    IPv6 = 6
}