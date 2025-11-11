using AuthSystem.Domain.Common.Base;
using AuthSystem.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object آدرس ایمیل
/// این کلاس مسئول اعتبارسنجی و مدیریت آدرس ایمیل است
/// </summary>
public sealed class Email : ValueObject
{
    /// <summary>
    /// حداکثر طول مجاز برای آدرس ایمیل
    /// </summary>
    private const int MaxLength = 255;

    /// <summary>
    /// الگوی regex برای اعتبارسنجی ایمیل
    /// </summary>
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(250));

    /// <summary>
    /// مقدار آدرس ایمیل
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// نام کاربری (قسمت قبل از @)
    /// </summary>
    public string Username => Value.Split('@')[0];

    /// <summary>
    /// دامنه (قسمت بعد از @)
    /// </summary>
    public string Domain => Value.Split('@')[1];

    /// <summary>
    /// آدرس ایمیل نرمال شده (حروف کوچک)
    /// </summary>
    public string NormalizedValue { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private Email(string value)
    {
        Value = value;
        NormalizedValue = value.ToLowerInvariant();
    }

    /// <summary>
    /// ایجاد نمونه معتبر از ایمیل
    /// </summary>
    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException("آدرس ایمیل نمی‌تواند خالی باشد");

        value = value.Trim();

        if (value.Length > MaxLength)
            throw new InvalidEmailException($"آدرس ایمیل نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

        if (!IsValidEmail(value))
            throw new InvalidEmailException($"فرمت آدرس ایمیل '{value}' نامعتبر است");

        return new Email(value);
    }

    /// <summary>
    /// ایجاد ایمیل بدون اعتبارسنجی (برای موارد خاص مثل بازیابی از دیتابیس)
    /// </summary>
    internal static Email CreateUnsafe(string value)
    {
        return new Email(value);
    }

    /// <summary>
    /// بررسی اعتبار فرمت ایمیل
    /// </summary>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            return EmailRegex.IsMatch(email);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    /// <summary>
    /// آیا این ایمیل از دامنه مشخصی است
    /// </summary>
    public bool IsFromDomain(string domain)
    {
        return Domain.Equals(domain, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// آیا این ایمیل موقت/یکبار مصرف است
    /// </summary>
    public bool IsDisposable()
    {
        // لیست دامنه‌های ایمیل موقت معروف
        var disposableDomains = new[]
        {
            "tempmail.com", "guerrillamail.com", "mailinator.com",
            "10minutemail.com", "throwaway.email", "yopmail.com"
        };

        return Array.Exists(disposableDomains,
            domain => Domain.EndsWith(domain, StringComparison.OrdinalIgnoreCase));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return NormalizedValue;
    }

    public override string ToString() => Value;

    /// <summary>
    /// تبدیل implicit از string به Email
    /// </summary>
    public static implicit operator string(Email email) => email.Value;
}