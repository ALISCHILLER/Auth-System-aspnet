using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object شماره تلفن
/// پشتیبانی از شماره‌های موبایل و ثابت ایران
/// </summary>
public sealed class PhoneNumber : ValueObject
{
    /// <summary>
    /// الگوی شماره موبایل ایران
    /// </summary>
    private static readonly Regex MobileRegex = new(
        @"^(\+98|0)?9\d{9}$",
        RegexOptions.Compiled);

    /// <summary>
    /// الگوی شماره تلفن ثابت ایران
    /// </summary>
    private static readonly Regex LandlineRegex = new(
        @"^(\+98|0)?(21|26|25|86|24|23|81|28|31|44|11|74|83|51|45|17|41|54|87|71|66|34|56|13|77|76|61|38|58|35|84)\d{8}$",
        RegexOptions.Compiled);

    /// <summary>
    /// پیش‌شماره‌های اپراتورهای موبایل
    /// </summary>
    private static readonly Dictionary<string, string> MobileOperators = new()
    {
        { "901", "همراه اول" }, { "902", "ایرانسل" }, { "903", "ایرانسل" },
        { "905", "همراه اول" }, { "930", "ایرانسل" }, { "933", "ایرانسل" },
        { "935", "ایرانسل" }, { "936", "ایرانسل" }, { "937", "ایرانсел" },
        { "938", "ایرانسل" }, { "939", "ایرانسل" }, { "910", "همراه اول" },
        { "911", "همراه اول" }, { "912", "همراه اول" }, { "913", "همراه اول" },
        { "914", "همراه اول" }, { "915", "همراه اول" }, { "916", "همراه اول" },
        { "917", "همراه اول" }, { "918", "همراه اول" }, { "919", "همراه اول" },
        { "990", "همراه اول" }, { "991", "همراه اول" }, { "992", "همراه اول" },
        { "993", "همراه اول" }, { "994", "همراه اول" }, { "932", "رایتل" },
        { "920", "رایتل" }, { "921", "رایتل" }, { "922", "شاتل موبایل" },
        { "998", "شاتل موبایل" }
    };

    /// <summary>
    /// مقدار شماره تلفن (همیشه با +98 شروع می‌شود)
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// نوع شماره (موبایل یا ثابت)
    /// </summary>
    public PhoneType Type { get; }

    /// <summary>
    /// شماره بدون کد کشور
    /// </summary>
    public string NationalNumber => Value.Substring(3);

    /// <summary>
    /// اپراتور موبایل (در صورت موبایل بودن)
    /// </summary>
    public string? Operator { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private PhoneNumber(string value, PhoneType type, string? @operator)
    {
        Value = value;
        Type = type;
        Operator = @operator;
    }

    /// <summary>
    /// ایجاد شماره تلفن معتبر
    /// </summary>
    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPhoneNumberException("شماره تلفن نمی‌تواند خالی باشد");

        // تمیز کردن شماره
        value = CleanPhoneNumber(value);

        // تبدیل به فرمت بین‌المللی
        value = NormalizeToInternational(value);

        // تعیین نوع و اعتبارسنجی
        if (IsMobile(value))
        {
            var @operator = GetOperator(value);
            return new PhoneNumber(value, PhoneType.Mobile, @operator);
        }

        if (IsLandline(value))
        {
            return new PhoneNumber(value, PhoneType.Landline, null);
        }

        throw new InvalidPhoneNumberException($"شماره تلفن '{value}' نامعتبر است");
    }

    /// <summary>
    /// تمیز کردن شماره از کاراکترهای اضافی
    /// </summary>
    private static string CleanPhoneNumber(string value)
    {
        // حذف فاصله، خط تیره و پرانتز
        value = value.Replace(" ", "")
                     .Replace("-", "")
                     .Replace("(", "")
                     .Replace(")", "");

        // تبدیل اعداد فارسی
        var persianNumbers = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
        var englishNumbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        for (var i = 0; i < persianNumbers.Length; i++)
        {
            value = value.Replace(persianNumbers[i], englishNumbers[i]);
        }
        return value;
    }

    /// <summary>
    /// تبدیل به فرمت بین‌المللی
    /// </summary>
    private static string NormalizeToInternational(string value)
    {
        if (value.StartsWith("0"))
            value = "+98" + value.Substring(1);
        else if (!value.StartsWith("+98"))
            value = "+98" + value;
        return value;
    }

    /// <summary>
    /// بررسی موبایل بودن
    /// </summary>
    private static bool IsMobile(string value)
    {
        return MobileRegex.IsMatch(value);
    }

    /// <summary>
    /// بررسی تلفن ثابت بودن
    /// </summary>
    private static bool IsLandline(string value)
    {
        return LandlineRegex.IsMatch(value);
    }

    /// <summary>
    /// تشخیص اپراتور موبایل
    /// </summary>
    private static string? GetOperator(string value)
    {
        var prefix = value.Substring(3, 3); // سه رقم بعد از +98
        return MobileOperators.TryGetValue(prefix, out var @operator) ? @operator : "نامشخص";
    }

    /// <summary>
    /// فرمت نمایشی شماره
    /// </summary>
    public string GetDisplayFormat()
    {
        if (Type == PhoneType.Mobile)
        {
            // +98 912 345 6789
            return $"{Value.Substring(0, 3)} {Value.Substring(3, 3)} {Value.Substring(6, 3)} {Value.Substring(9)}";
        }

        // برای تلفن ثابت
        var cityCode = Value.Substring(3, 2);
        var number = Value.Substring(5);
        return $"{Value.Substring(0, 3)} {cityCode} {number.Substring(0, 4)} {number.Substring(4)}";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}