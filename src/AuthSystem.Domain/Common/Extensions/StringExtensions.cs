using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// اکستنشن‌های رشته
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// آیا رشته خالی است
    /// </summary>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// آیا رشته خالی یا شامل فضای خالی است
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// تبدیل به حروف کوچک با اطمینان از عدم خالی بودن
    /// </summary>
    public static string ToLowerSafe(this string value)
    {
        return value?.ToLowerInvariant() ?? string.Empty;
    }

    /// <summary>
    /// تبدیل به حروف بزرگ با اطمینان از عدم خالی بودن
    /// </summary>
    public static string ToUpperSafe(this string value)
    {
        return value?.ToUpperInvariant() ?? string.Empty;
    }

    /// <summary>
    /// بریدن رشته با اطمینان از عدم خالی بودن
    /// </summary>
    public static string SafeSubstring(this string value, int startIndex, int length)
    {
        if (value.IsNullOrEmpty())
            return string.Empty;

        if (startIndex >= value.Length)
            return string.Empty;

        if (startIndex + length > value.Length)
            length = value.Length - startIndex;

        return value.Substring(startIndex, length);
    }

    /// <summary>
    /// حذف کاراکترهای غیر الفبا و عدد
    /// </summary>
    public static string RemoveNonAlphanumeric(this string value)
    {
        return Regex.Replace(value ?? string.Empty, "[^a-zA-Z0-9]", "");
    }

    /// <summary>
    /// تبدیل اعداد فارسی به انگلیسی
    /// </summary>
    public static string ToEnglishNumbers(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var persianNumbers = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
        var englishNumbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        foreach (var i in Enumerable.Range(0, persianNumbers.Length))
        {
            text = text.Replace(persianNumbers[i], englishNumbers[i]);
        }

        return text;
    }

    /// <summary>
    /// هش کردن رشته
    /// </summary>
    public static string ToSha256Hash(this string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// رمزگشایی Base64
    /// </summary>
    public static string FromBase64(this string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
            return string.Empty;

        try
        {
            var bytes = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return base64String;
        }
    }

    /// <summary>
    /// رمزنگاری Base64
    /// </summary>
    public static string ToBase64(this string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        var bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(bytes);
    }
}