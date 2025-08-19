using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// اکستنشن‌های آدرس ایمیل
/// </summary>
public static class EmailExtensions
{
    /// <summary>
    /// تبدیل به Email
    /// </summary>
    public static Email ToEmail(this string value)
    {
        return Email.Create(value);
    }

    /// <summary>
    /// آیا ایمیل معتبر است
    /// </summary>
    public static bool IsValidEmail(this string value)
    {
        return Email.IsValidEmail(value);
    }

    /// <summary>
    /// آیا ایمیل از دامنه مشخصی است
    /// </summary>
    public static bool IsFromDomain(this Email email, string domain)
    {
        return email.IsFromDomain(domain);
    }

    /// <summary>
    /// آیا ایمیل موقت است
    /// </summary>
    public static bool IsDisposable(this Email email)
    {
        return email.IsDisposable();
    }

    /// <summary>
    /// نرمال‌سازی ایمیل
    /// </summary>
    public static string NormalizeEmail(this string email)
    {
        return email?.Trim().ToLowerInvariant() ?? string.Empty;
    }

    /// <summary>
    /// دریافت دامنه ایمیل
    /// </summary>
    public static string GetEmailDomain(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return string.Empty;

        var parts = email.Split('@');
        return parts.Length > 1 ? parts[1] : string.Empty;
    }

    /// <summary>
    /// دریافت نام کاربری ایمیل
    /// </summary>
    public static string GetUsername(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return string.Empty;

        var parts = email.Split('@');
        return parts[0];
    }
}