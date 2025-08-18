using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object کد ملی ایرانی
/// این کلاس مسئول اعتبارسنجی و مدیریت کد ملی ایرانی است
/// </summary>
public sealed class NationalCode : ValueObject
{
    /// <summary>
    /// طول استاندارد کد ملی
    /// </summary>
    private const int Length = 10;

    /// <summary>
    /// کدهای ملی غیرمجاز (همه ارقام یکسان)
    /// </summary>
    private static readonly string[] InvalidCodes =
    {
        "0000000000", "1111111111", "2222222222", "3333333333",
        "4444444444", "5555555555", "6666666666", "7777777777",
        "8888888888", "9999999999"
    };

    /// <summary>
    /// مقدار کد ملی (همیشه 10 رقم)
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// کد منطقه (3 رقم اول)
    /// </summary>
    public string RegionCode => Value.Substring(0, 3);

    /// <summary>
    /// آیا کد ملی مربوط به اتباع خارجی است
    /// </summary>
    public bool IsForeigner => Value.StartsWith("9");

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private NationalCode(string value)
    {
        Value = value;
    }

    /// <summary>
    /// ایجاد نمونه معتبر از کد ملی
    /// </summary>
    public static NationalCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw InvalidNationalCodeException.ForEmptyCode();

        // تمیز کردن ورودی
        value = CleanInput(value);

        if (value.Length != Length)
            throw InvalidNationalCodeException.ForInvalidLength(value, Length);

        if (!value.All(char.IsDigit))
            throw InvalidNationalCodeException.ForInvalidFormat(value);

        if (InvalidCodes.Contains(value))
            throw InvalidNationalCodeException.ForRepeatingDigits(value);

        if (!IsValidNationalCode(value))
            throw InvalidNationalCodeException.ForInvalidChecksum(value);

        return new NationalCode(value);
    }

    /// <summary>
    /// تمیز کردن ورودی (حذف فاصله، خط تیره و تبدیل اعداد فارسی)
    /// </summary>
    private static string CleanInput(string value)
    {
        value = value.Trim().Replace(" ", "").Replace("-", "");

        // تبدیل اعداد فارسی/عربی به انگلیسی
        var persianNumbers = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
        var arabicNumbers = new[] { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };
        var englishNumbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        for (int i = 0; i < 10; i++)
        {
            value = value.Replace(persianNumbers[i], englishNumbers[i]);
            value = value.Replace(arabicNumbers[i], englishNumbers[i]);
        }

        return value;
    }

    /// <summary>
    /// بررسی اعتبار کد ملی با الگوریتم استاندارد
    /// </summary>
    private static bool IsValidNationalCode(string code)
    {
        var check = int.Parse(code[9].ToString());
        var sum = 0;
        for (var i = 0; i < 9; i++)
        {
            sum += int.Parse(code[i].ToString()) * (10 - i);
        }
        var remainder = sum % 11;
        return (remainder < 2 && check == remainder) ||
               (remainder >= 2 && check == 11 - remainder);
    }

    /// <summary>
    /// فرمت نمایشی کد ملی (xxx-xxxxxx-x)
    /// </summary>
    public string GetFormattedValue()
    {
        return $"{Value.Substring(0, 3)}-{Value.Substring(3, 6)}-{Value[9]}";
    }

    /// <summary>
    /// بررسی برابری با کد ملی دیگر
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// نمایش مقدار کد ملی
    /// </summary>
    public override string ToString() => Value;
}