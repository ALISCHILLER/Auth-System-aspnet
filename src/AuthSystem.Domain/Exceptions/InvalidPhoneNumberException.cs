using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای شماره تلفن نامعتبر
/// </summary>
[Serializable]
public sealed class InvalidPhoneNumberException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.PHONE.INVALID";

    /// <summary>
    /// شماره تلفن نامعتبر
    /// </summary>
    public string? InvalidPhoneNumber { get; private set; }

    /// <summary>
    /// کد کشور
    /// </summary>
    public string? CountryCode { get; private set; }

    /// <summary>
    /// منطقه/کشور
    /// </summary>
    public string? Region { get; private set; }

    /// <summary>
    /// نوع خطا
    /// </summary>
    public PhoneNumberErrorType ErrorType { get; private set; }

    private InvalidPhoneNumberException(
        string message,
        string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
        ErrorType = PhoneNumberErrorType.InvalidFormat;
    }

    private InvalidPhoneNumberException(
        string message,
        Exception innerException,
        string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode, innerException)
    {
        ErrorType = PhoneNumberErrorType.InvalidFormat;
    }

    /// <summary>
    /// متد WithDetail با بازگشت نوع صحیح
    /// </summary>
    public new InvalidPhoneNumberException WithDetail(string key, object value)
    {
        base.WithDetail(key, value);
        return this;
    }

    /// <summary>
    /// ایجاد استثنا برای شماره تلفن خالی
    /// </summary>
    public static InvalidPhoneNumberException ForEmptyPhoneNumber()
    {
        return new InvalidPhoneNumberException(
            "شماره تلفن نمی‌تواند خالی باشد.",
            "AUTH.PHONE.EMPTY"
        )
        {
            ErrorType = PhoneNumberErrorType.Empty
        };
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت نامعتبر
    /// </summary>
    public static InvalidPhoneNumberException ForInvalidFormat(string phoneNumber)
    {
        return new InvalidPhoneNumberException(
            $"فرمت شماره تلفن '{phoneNumber}' نامعتبر است.",
            "AUTH.PHONE.INVALID_FORMAT"
        )
        {
            InvalidPhoneNumber = phoneNumber,
            ErrorType = PhoneNumberErrorType.InvalidFormat
        }
        .WithDetail(nameof(InvalidPhoneNumber), phoneNumber);
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت نامعتبر با استثنای داخلی
    /// </summary>
    public static InvalidPhoneNumberException ForInvalidFormat(string phoneNumber, string message, Exception innerException)
    {
        return new InvalidPhoneNumberException(
            message,
            innerException,
            "AUTH.PHONE.INVALID_FORMAT"
        )
        {
            InvalidPhoneNumber = phoneNumber,
            ErrorType = PhoneNumberErrorType.InvalidFormat
        }
        .WithDetail(nameof(InvalidPhoneNumber), phoneNumber);
    }

    /// <summary>
    /// ایجاد استثنا برای شماره نامعتبر
    /// </summary>
    public static InvalidPhoneNumberException ForInvalidNumber(string phoneNumber)
    {
        return new InvalidPhoneNumberException(
            $"شماره تلفن '{phoneNumber}' نامعتبر است.",
            "AUTH.PHONE.INVALID_NUMBER"
        )
        {
            InvalidPhoneNumber = phoneNumber,
            ErrorType = PhoneNumberErrorType.InvalidNumber
        }
        .WithDetail(nameof(InvalidPhoneNumber), phoneNumber);
    }

    /// <summary>
    /// ایجاد استثنا برای کد کشور نامعتبر
    /// </summary>
    public static InvalidPhoneNumberException ForInvalidCountryCode(string phoneNumber, string countryCode)
    {
        return new InvalidPhoneNumberException(
            $"کد کشور '{countryCode}' برای شماره '{phoneNumber}' نامعتبر است.",
            "AUTH.PHONE.INVALID_COUNTRY_CODE"
        )
        {
            InvalidPhoneNumber = phoneNumber,
            CountryCode = countryCode,
            ErrorType = PhoneNumberErrorType.InvalidCountryCode
        }
        .WithDetail(nameof(InvalidPhoneNumber), phoneNumber)
        .WithDetail(nameof(CountryCode), countryCode);
    }

    /// <summary>
    /// ایجاد استثنا برای منطقه نامعتبر
    /// </summary>
    public static InvalidPhoneNumberException ForInvalidRegion(string phoneNumber, string region)
    {
        return new InvalidPhoneNumberException(
            $"منطقه '{region}' برای شماره '{phoneNumber}' نامعتبر است.",
            "AUTH.PHONE.INVALID_REGION"
        )
        {
            InvalidPhoneNumber = phoneNumber,
            Region = region,
            ErrorType = PhoneNumberErrorType.InvalidRegion
        }
        .WithDetail(nameof(InvalidPhoneNumber), phoneNumber)
        .WithDetail(nameof(Region), region);
    }

    /// <summary>
    /// ایجاد استثنا برای شماره تکراری
    /// </summary>
    public static InvalidPhoneNumberException ForDuplicateNumber(string phoneNumber)
    {
        return new InvalidPhoneNumberException(
            $"شماره تلفن '{phoneNumber}' قبلاً ثبت شده است.",
            "AUTH.PHONE.DUPLICATE"
        )
        {
            InvalidPhoneNumber = phoneNumber,
            ErrorType = PhoneNumberErrorType.Duplicate
        }
        .WithDetail(nameof(InvalidPhoneNumber), phoneNumber);
    }
}

/// <summary>
/// انواع خطاهای شماره تلفن
/// </summary>
public enum PhoneNumberErrorType
{
    Empty,
    InvalidFormat,
    InvalidNumber,
    InvalidCountryCode,
    InvalidRegion,
    Duplicate,
    TooShort,
    TooLong
}
