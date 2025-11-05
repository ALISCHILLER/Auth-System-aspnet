using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common.Base;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object کد ملی ایرانی
/// این کلاس مسئول اعتبارسنجی و مدیریت کد ملی ایرانی است
/// </summary>
public sealed class NationalCode : ValueObject
{
    /// <summary>
    /// طول کد ملی
    /// </summary>
    private const int Length = 10;

    /// <summary>
    /// کدهای ملی غیرمجاز
    /// </summary>
    private static readonly string[] InvalidCodes =
    {
        "0000000000", "1111111111", "2222222222", "3333333333",
        "4444444444", "5555555555", "6666666666", "7777777777",
        "8888888888", "9999999999"
    };

    /// <summary>
    /// مقدار کد ملی
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// کد منطقه (3 رقم اول)
    /// </summary>
    public string RegionCode => Value.Substring(0, 3);

    /// <summary>
    /// آیا کد ملی برای اتباع خارجی است
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
            throw new InvalidNationalCodeException("کد ملی نمی‌تواند خالی باشد");

        // حذف فاصله‌ها و کاراکترهای اضافی
        value = value.Trim().Replace(" ", "").Replace("-", "");

        // تبدیل اعداد فارسی به انگلیسی
        value = ConvertPersianToEnglishNumbers(value);

        if (value.Length != Length)
            throw new InvalidNationalCodeException($"کد ملی باید {Length} رقم باشد");

        if (!value.All(char.IsDigit))
            throw new InvalidNationalCodeException("کد ملی فقط باید شامل اعداد باشد");

        if (InvalidCodes.Contains(value))
            throw new InvalidNationalCodeException("کد ملی وارد شده نامعتبر است");

        if (!IsValidNationalCode(value))
            throw new InvalidNationalCodeException("کد ملی نامعتبر است");

        return new NationalCode(value);
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
    /// تبدیل اعداد فارسی به انگلیسی
    /// </summary>
    private static string ConvertPersianToEnglishNumbers(string input)
    {
        var persianNumbers = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
        var englishNumbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        for (var i = 0; i < persianNumbers.Length; i++)
        {
            input = input.Replace(persianNumbers[i], englishNumbers[i]);
        }
        return input;
    }

    /// <summary>
    /// فرمت نمایشی کد ملی (xxx-xxxxxx-x)
    /// </summary>
    public string GetFormattedValue()
    {
        return $"{Value.Substring(0, 3)}-{Value.Substring(3, 6)}-{Value[9]}";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}