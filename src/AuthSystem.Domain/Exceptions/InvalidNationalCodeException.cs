using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای کد ملی نامعتبر
/// </summary>
[Serializable]
public sealed class InvalidNationalCodeException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.NATIONALCODE.INVALID";

    /// <summary>
    /// کد ملی نامعتبر
    /// </summary>
    public string? InvalidNationalCode { get; private set; }

    /// <summary>
    /// نوع خطا
    /// </summary>
    public NationalCodeError ErrorType { get; private set; }

    private InvalidNationalCodeException(string message, string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
        ErrorType = NationalCodeError.General;
    }

    private InvalidNationalCodeException(string? nationalCode, string message, NationalCodeError errorType = NationalCodeError.General, string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
        InvalidNationalCode = nationalCode;
        ErrorType = errorType;

        if (!string.IsNullOrWhiteSpace(nationalCode))
            WithDetail(nameof(InvalidNationalCode), nationalCode);

        WithDetail(nameof(ErrorType), errorType.ToString());
    }

    /// <summary>
    /// ایجاد استثنا برای کد ملی خالی
    /// </summary>
    public static InvalidNationalCodeException ForEmptyCode()
    {
        return new InvalidNationalCodeException(
            null,
            "کد ملی نمی‌تواند خالی باشد.",
            NationalCodeError.InvalidFormat,
            "AUTH.NATIONALCODE.EMPTY"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای طول نامعتبر
    /// </summary>
    public static InvalidNationalCodeException ForInvalidLength(string nationalCode, int expectedLength)
    {
        return new InvalidNationalCodeException(
            nationalCode,
            $"کد ملی باید دقیقاً {expectedLength} رقم باشد. کد وارد شده دارای {nationalCode?.Length ?? 0} رقم است.",
            NationalCodeError.InvalidLength,
            "AUTH.NATIONALCODE.INVALID_LENGTH"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت نامعتبر
    /// </summary>
    public static InvalidNationalCodeException ForInvalidFormat(string nationalCode)
    {
        return new InvalidNationalCodeException(
            nationalCode,
            $"کد ملی '{nationalCode}' باید فقط شامل ارقام باشد.",
            NationalCodeError.InvalidFormat,
            "AUTH.NATIONALCODE.INVALID_FORMAT"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای چک‌سام نامعتبر
    /// </summary>
    public static InvalidNationalCodeException ForInvalidChecksum(string nationalCode)
    {
        return new InvalidNationalCodeException(
            nationalCode,
            $"کد ملی '{nationalCode}' از نظر الگوریتم اعتبارسنجی کد ملی ایران نامعتبر است.",
            NationalCodeError.InvalidChecksum,
            "AUTH.NATIONALCODE.INVALID_CHECKSUM"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای کد ملی تکراری
    /// </summary>
    public static InvalidNationalCodeException ForRepeatingDigits(string nationalCode)
    {
        return new InvalidNationalCodeException(
            nationalCode,
            $"کد ملی نمی‌تواند از ارقام تکراری مانند '{nationalCode}' تشکیل شده باشد.",
            NationalCodeError.RepeatingDigits,
            "AUTH.NATIONALCODE.REPEATING_DIGITS"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای کد ملی مسدود شده
    /// </summary>
    public static InvalidNationalCodeException ForBlockedCode(string nationalCode)
    {
        return new InvalidNationalCodeException(
            nationalCode,
            $"کد ملی '{nationalCode}' مسدود شده است و قابل استفاده نیست.",
            NationalCodeError.Blocked,
            "AUTH.NATIONALCODE.BLOCKED"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای کد ملی تکراری در سیستم
    /// </summary>
    public static InvalidNationalCodeException ForDuplicateCode(string nationalCode)
    {
        return new InvalidNationalCodeException(
            nationalCode,
            $"کد ملی '{nationalCode}' قبلاً در سیستم ثبت شده است.",
            NationalCodeError.Duplicate,
            "AUTH.NATIONALCODE.DUPLICATE"
        );
    }
}

/// <summary>
/// انواع خطاهای کد ملی
/// </summary>
public enum NationalCodeError
{
    General,
    InvalidLength,
    InvalidFormat,
    InvalidChecksum,
    RepeatingDigits,
    Blocked,
    Duplicate
}