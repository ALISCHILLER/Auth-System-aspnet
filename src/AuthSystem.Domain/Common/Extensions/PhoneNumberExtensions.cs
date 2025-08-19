using AuthSystem.Domain.Enums;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// اکستنشن‌های شماره تلفن
/// </summary>
public static class PhoneNumberExtensions
{
    /// <summary>
    /// تبدیل به PhoneNumber
    /// </summary>
    public static PhoneNumber ToPhoneNumber(this string value)
    {
        return PhoneNumber.Create(value);
    }

    /// <summary>
    /// آیا شماره تلفن معتبر است
    /// </summary>
    public static bool IsValidPhoneNumber(this string value)
    {
        try
        {
            PhoneNumber.Create(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// دریافت فرمت نمایشی شماره تلفن
    /// </summary>
    public static string GetDisplayFormat(this PhoneNumber phoneNumber)
    {
        return phoneNumber.GetDisplayFormat();
    }

    /// <summary>
    /// آیا شماره تلفن موبایل است
    /// </summary>
    public static bool IsMobileNumber(this PhoneNumber phoneNumber)
    {
        return phoneNumber.Type == PhoneType.Mobile;
    }

    /// <summary>
    /// آیا شماره تلفن ثابت است
    /// </summary>
    public static bool IsLandlineNumber(this PhoneNumber phoneNumber)
    {
        return phoneNumber.Type == PhoneType.Landline;
    }

    /// <summary>
    /// دریافت اپراتور موبایل
    /// </summary>
    public static string GetOperator(this PhoneNumber phoneNumber)
    {
        return phoneNumber.Operator ?? "نامشخص";
    }

    /// <summary>
    /// دریافت شماره بدون کد کشور
    /// </summary>
    public static string GetNationalNumber(this PhoneNumber phoneNumber)
    {
        return phoneNumber.NationalNumber;
    }

    /// <summary>
    /// دریافت شماره تلفن به فرمت بین‌المللی
    /// </summary>
    public static string ToInternationalFormat(this string phoneNumber)
    {
        try
        {
            return PhoneNumber.Create(phoneNumber).Value;
        }
        catch
        {
            return phoneNumber;
        }
    }
}