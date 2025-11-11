using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای شماره تلفن نامعتبر
/// این استثنا زمانی رخ می‌دهد که فرمت شماره تلفن صحیح نباشد
/// </summary>
public class InvalidPhoneNumberException : DomainException
{
    /// <summary>
    /// شماره تلفن نامعتبر
    /// </summary>
    public string PhoneNumber { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidPhoneNumber";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidPhoneNumberException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidPhoneNumberException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با شماره تلفن و پیام خطا
    /// </summary>
    public InvalidPhoneNumberException(string phoneNumber, string message)
        : this(message)
    {
        PhoneNumber = phoneNumber;
    }

    /// <summary>
    /// ایجاد استثنا برای شماره تلفن خالی
    /// </summary>
    public static InvalidPhoneNumberException ForEmptyPhoneNumber()
    {
        return new InvalidPhoneNumberException("شماره تلفن نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت شماره تلفن نامعتبر
    /// </summary>
    public static InvalidPhoneNumberException ForInvalidFormat(string phoneNumber)
    {
        return new InvalidPhoneNumberException(phoneNumber, $"فرمت شماره تلفن '{phoneNumber}' نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای شماره تلفن غیراستاندارد
    /// </summary>
    public static InvalidPhoneNumberException ForNonStandardNumber(string phoneNumber)
    {
        return new InvalidPhoneNumberException(phoneNumber, $"شماره تلفن '{phoneNumber}' غیراستاندارد است");
    }

    /// <summary>
    /// ایجاد استثنا برای شماره تلفن تکراری
    /// </summary>
    public static InvalidPhoneNumberException ForDuplicatePhoneNumber(string phoneNumber)
    {
        return new InvalidPhoneNumberException(phoneNumber, $"شماره تلفن '{phoneNumber}' قبلاً ثبت شده است");
    }

    /// <summary>
    /// ایجاد استثنا برای شماره تلفن غیرفعال
    /// </summary>
    public static InvalidPhoneNumberException ForInactivePhoneNumber(string phoneNumber)
    {
        return new InvalidPhoneNumberException(phoneNumber, $"شماره تلفن '{phoneNumber}' غیرفعال است");
    }
}