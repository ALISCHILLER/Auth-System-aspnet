using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای کد ملی نامعتبر
/// این استثنا زمانی رخ می‌دهد که فرمت کد ملی صحیح نباشد
/// </summary>
public class InvalidNationalCodeException : DomainException
{
    /// <summary>
    /// کد ملی نامعتبر
    /// </summary>
    public string NationalCode { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidNationalCode";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidNationalCodeException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidNationalCodeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با کد ملی و پیام خطا
    /// </summary>
    public InvalidNationalCodeException(string nationalCode, string message)
        : this(message)
    {
        NationalCode = nationalCode;
    }

    /// <summary>
    /// ایجاد استثنا برای کد ملی خالی
    /// </summary>
    public static InvalidNationalCodeException ForEmptyNationalCode()
    {
        return new InvalidNationalCodeException("کد ملی نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای طول نامناسب کد ملی
    /// </summary>
    public static InvalidNationalCodeException ForInvalidLength(int expectedLength)
    {
        return new InvalidNationalCodeException($"کد ملی باید {expectedLength} رقم باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت کد ملی نامعتبر
    /// </summary>
    public static InvalidNationalCodeException ForInvalidFormat(string nationalCode)
    {
        return new InvalidNationalCodeException(nationalCode, $"فرمت کد ملی '{nationalCode}' نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای کد ملی تکراری
    /// </summary>
    public static InvalidNationalCodeException ForDuplicateNationalCode(string nationalCode)
    {
        return new InvalidNationalCodeException(nationalCode, $"کد ملی '{nationalCode}' قبلاً ثبت شده است");
    }

    /// <summary>
    /// ایجاد استثنا برای کد ملی نامعتبر
    /// </summary>
    public static InvalidNationalCodeException ForInvalidChecksum(string nationalCode)
    {
        return new InvalidNationalCodeException(nationalCode, $"کد ملی '{nationalCode}' نامعتبر است");
    }
}