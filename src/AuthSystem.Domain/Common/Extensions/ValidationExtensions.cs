using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// اکستنشن‌های اعتبارسنجی
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// اعتبارسنجی شیء با استفاده از DataAnnotations
    /// </summary>
    public static void Validate(this object instance)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(instance);

        if (!Validator.TryValidateObject(instance, validationContext, validationResults, true))
        {
            var errors = validationResults
                .Select(r => r.ErrorMessage)
                .Where(m => !string.IsNullOrEmpty(m))
                .ToList();

            // تبدیل لیست پیام‌های خطا به یک رشته
            var errorMessage = string.Join("; ", errors);
            throw new ValidationException(errorMessage);
        }
    }

    /// <summary>
    /// اعتبارسنجی شیء با استفاده از DataAnnotations (ناهمزمان)
    /// </summary>
    public static async Task ValidateAsync(this object instance)
    {
        // استفاده از Task.Run برای شبیه‌سازی رفتار ناهمزمان
        await Task.Run(() => instance.Validate());
    }

    /// <summary>
    /// اعتبارسنجی شرطی
    /// </summary>
    public static void ValidateIf(this object instance, bool condition)
    {
        if (condition)
            instance.Validate();
    }

    /// <summary>
    /// اعتبارسنجی شرطی (ناهمزمان)
    /// </summary>
    public static async Task ValidateIfAsync(this object instance, bool condition)
    {
        if (condition)
            await instance.ValidateAsync();
    }

    /// <summary>
    /// اعتبارسنجی رمز عبور
    /// </summary>
    public static void ValidatePassword(this string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("رمز عبور نمی‌تواند خالی باشد");

        if (password.Length < 8)
            throw new ArgumentException("رمز عبور باید حداقل 8 کاراکتر باشد");

        if (!Regex.IsMatch(password, @"[A-Z]"))
            throw new ArgumentException("رمز عبور باید حداقل یک حرف بزرگ داشته باشد");

        if (!Regex.IsMatch(password, @"[a-z]"))
            throw new ArgumentException("رمز عبور باید حداقل یک حرف کوچک داشته باشد");

        if (!Regex.IsMatch(password, @"\d"))
            throw new ArgumentException("رمز عبور باید حداقل یک عدد داشته باشد");

        if (!Regex.IsMatch(password, @"[\W_]"))
            throw new ArgumentException("رمز عبور باید حداقل یک کاراکتر خاص داشته باشد");
    }

    /// <summary>
    /// اعتبارسنجی آدرس ایمیل
    /// </summary>
    public static void ValidateEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("آدرس ایمیل نمی‌تواند خالی باشد");

        if (email.Length > 255)
            throw new ArgumentException("آدرس ایمیل نمی‌تواند بیشتر از 255 کاراکتر باشد");

        var emailRegex = new Regex(
            @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250));

        if (!emailRegex.IsMatch(email))
            throw new ArgumentException("فرمت آدرس ایمیل نامعتبر است");
    }

    /// <summary>
    /// اعتبارسنجی کد ملی
    /// </summary>
    public static void ValidateNationalCode(this string nationalCode)
    {
        if (string.IsNullOrWhiteSpace(nationalCode))
            throw new ArgumentException("کد ملی نمی‌تواند خالی باشد");

        nationalCode = nationalCode.Trim().Replace(" ", "").Replace("-", "");
        nationalCode = nationalCode.ToEnglishNumbers();

        if (nationalCode.Length != 10)
            throw new ArgumentException("کد ملی باید 10 رقم باشد");

        if (!nationalCode.All(char.IsDigit))
            throw new ArgumentException("کد ملی فقط باید شامل اعداد باشد");

        var invalidCodes = new[] {
            "0000000000", "1111111111", "2222222222", "3333333333",
            "4444444444", "5555555555", "6666666666", "7777777777",
            "8888888888", "9999999999"
        };

        if (invalidCodes.Contains(nationalCode))
            throw new ArgumentException("کد ملی وارد شده نامعتبر است");

        var check = int.Parse(nationalCode[9].ToString());
        var sum = 0;
        for (var i = 0; i < 9; i++)
        {
            sum += int.Parse(nationalCode[i].ToString()) * (10 - i);
        }
        var remainder = sum % 11;

        if ((remainder < 2 && check != remainder) ||
            (remainder >= 2 && check != 11 - remainder))
        {
            throw new ArgumentException("کد ملی نامعتبر است");
        }
    }
}