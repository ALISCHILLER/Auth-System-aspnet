using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using System.Text;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object شماره تلفن ایرانی
/// این کلاس مسئول اعتبارسنجی و مدیریت شماره تلفن ایرانی است
/// </summary>
public class PhoneNumber : ValueObject
{
    /// <summary>
    /// مقدار شماره تلفن
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// سازنده خصوصی برای ایجاد نمونه از طریق متد Create
    /// </summary>
    private PhoneNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد یک نمونه معتبر از شماره تلفن
    /// این متد قبل از ساخت نمونه، اعتبارسنجی لازم را انجام می‌دهد
    /// </summary>
    /// <param name="value">مقدار شماره تلفن وارد شده</param>
    /// <returns>یک نمونه معتبر از کلاس PhoneNumber</returns>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن مقدار</exception>
    /// <exception cref="InvalidPhoneNumberException">در صورت نامعتبر بودن فرمت شماره تلفن</exception>
    public static PhoneNumber Create(string value)
    {
        // بررسی خالی بودن مقدار
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value), "شماره تلفن نمی‌تواند خالی باشد");

        // حذف فاصله و کاراکترهای غیرعددی
        var cleanNumber = Normalize(value);

        // بررسی فرمت شماره تلفن ایرانی
        if (!IsValidIranianPhoneNumber(cleanNumber))
            throw new InvalidPhoneNumberException(value);

        return new PhoneNumber(cleanNumber);
    }

    /// <summary>
    /// بررسی اعتبار فرمت شماره تلفن ایرانی
    /// </summary>
    /// <param name="phoneNumber">شماره تلفن برای بررسی</param>
    /// <returns>در صورت معتبر بودن فرمت، true باز می‌گرداند</returns>
    private static bool IsValidIranianPhoneNumber(string phoneNumber)
    {
        // بررسی طول شماره
        if (phoneNumber.Length != 11)
            return false;

        // بررسی پیش‌شماره (باید با 09 شروع شود)
        return phoneNumber.StartsWith("09");
    }

    /// <summary>
    /// نرمال‌سازی شماره تلفن (حذف فاصله و کاراکترهای غیرعددی)
    /// </summary>
    /// <param name="phoneNumber">شماره تلفن ورودی</param>
    /// <returns>شماره تلفن نرمال‌سازی شده</returns>
    private static string Normalize(string phoneNumber)
    {
        var sb = new StringBuilder();
        foreach (var c in phoneNumber)
        {
            if (char.IsDigit(c))
                sb.Append(c);
        }
        return sb.ToString();
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