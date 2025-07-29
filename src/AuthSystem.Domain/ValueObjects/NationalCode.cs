using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using System;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object کد ملی ایرانی
/// این کلاس مسئول اعتبارسنجی و مدیریت کد ملی ایرانی است
/// </summary>
public class NationalCode : ValueObject
{
    /// <summary>
    /// مقدار کد ملی
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// سازنده خصوصی برای ایجاد نمونه از طریق متد Create
    /// </summary>
    private NationalCode(string value)
    {
        Value = value;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد یک نمونه معتبر از کد ملی
    /// این متد قبل از ساخت نمونه، اعتبارسنجی لازم را انجام می‌دهد
    /// </summary>
    /// <param name="value">مقدار کد ملی وارد شده</param>
    /// <returns>یک نمونه معتبر از کلاس NationalCode</returns>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن مقدار</exception>
    /// <exception cref="InvalidNationalCodeException">در صورت نامعتبر بودن فرمت کد ملی</exception>
    public static NationalCode Create(string value)
    {
        // بررسی خالی بودن مقدار
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value), "کد ملی نمی‌تواند خالی باشد");

        // بررسی طول کد ملی
        if (value.Length != 10)
            throw new InvalidNationalCodeException(value, "کد ملی باید 10 رقم باشد");

        // بررسی فرمت کد ملی ایرانی
        if (!IsValidNationalCode(value))
            throw new InvalidNationalCodeException(value, "کد ملی نامعتبر است");

        return new NationalCode(value);
    }

    /// <summary>
    /// بررسی اعتبار فرمت کد ملی ایرانی
    /// </summary>
    /// <param name="code">کد ملی برای بررسی</param>
    /// <returns>در صورت معتبر بودن فرمت، true باز می‌گرداند</returns>
    private static bool IsValidNationalCode(string code)
    {
        try
        {
            // اعتبارسنجی کد ملی ایران
            var check = int.Parse(code[9].ToString());
            var sum = 0;
            for (var i = 0; i < 9; i++)
            {
                sum += int.Parse(code[i].ToString()) * (10 - i);
            }
            var remainder = sum % 11;

            // بررسی بر اساس الگوریتم کد ملی ایران
            return (remainder < 2 && check == remainder) ||
                   (remainder >= 2 && check == 11 - remainder);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// دریافت کامپوننت‌های مورد نیاز برای مقایسه برابری
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// تبدیل به رشته
    /// </summary>
    public override string ToString() => Value;
}