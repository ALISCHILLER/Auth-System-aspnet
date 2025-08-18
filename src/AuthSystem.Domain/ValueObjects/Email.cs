using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object آدرس ایمیل با قابلیت نرمال‌سازی و پشتیبانی بین‌المللی
/// </summary>
public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Value { get; }
    public string NormalizedValue { get; }
    public string Domain { get; }
    public string Username { get; }

    private Email(string value)
    {
        Value = value;
        NormalizedValue = NormalizeEmail(value);

        var parts = NormalizedValue.Split('@');
        Username = parts[0];
        Domain = parts[1];
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw InvalidEmailException.ForEmptyEmail(); // استفاده از factory method

        value = value.Trim();

        if (value.Length > 254)
            throw InvalidEmailException.ForInvalidLength(value); // استفاده از factory method

        if (!IsValidEmail(value))
            throw InvalidEmailException.ForInvalidFormat(value); // استفاده از factory method

        return new Email(value);
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var idn = new IdnMapping();
            var parts = email.Split('@');
            if (parts.Length != 2) return false;

            parts[1] = idn.GetAscii(parts[1]);
            email = string.Join("@", parts);

            return EmailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    private static string NormalizeEmail(string email)
    {
        var parts = email.Split('@');
        var username = parts[0].ToLowerInvariant();
        var domain = parts[1].ToLowerInvariant();

        // نرمال‌سازی Gmail
        if (IsGmailDomain(domain))
        {
            username = username.Replace(".", "");
            var plusIndex = username.IndexOf('+');
            if (plusIndex > 0)
                username = username.Substring(0, plusIndex);
        }

        return $"{username}@{domain}";
    }

    private static bool IsGmailDomain(string domain)
    {
        return domain switch
        {
            "gmail.com" => true,
            "googlemail.com" => true,
            _ => false
        };
    }

    /// <summary>
    /// بررسی اینکه آیا دامنه از نوع ایمیل موقت است یا خیر
    /// </summary>
    public bool IsDisposableEmail()
    {
        // لیست نمونه از دامنه‌های ایمیل موقت
        var disposableDomains = new[]
        {
            "tempmail.com", "throwaway.email", "guerrillamail.com",
            "mailinator.com", "10minutemail.com", "yopmail.com"
        };

        return disposableDomains.Contains(Domain, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// بررسی اینکه آیا دامنه مسدود شده است یا خیر
    /// </summary>
    public bool IsBlockedDomain(IEnumerable<string> blockedDomains)
    {
        return blockedDomains.Contains(Domain, StringComparer.OrdinalIgnoreCase);
    }

    protected override IEnumerable<object> GetEqualityComponents() => new[] { NormalizedValue };
    public override string ToString() => Value;

    /// <summary>
    /// اپراتور تبدیل ضمنی از Email به string
    /// </summary>
    public static implicit operator string(Email email) => email.Value;
}
