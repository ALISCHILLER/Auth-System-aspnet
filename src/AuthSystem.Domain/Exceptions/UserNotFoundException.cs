using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای عدم یافتن کاربر
/// </summary>
[Serializable]
public sealed class UserNotFoundException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.USER.NOT_FOUND";

    /// <summary>
    /// شناسه کاربر (در صورت جستجو با شناسه)
    /// </summary>
    public Guid? UserId { get; private set; } // اضافه کردن private setter

    /// <summary>
    /// ایمیل کاربر (در صورت جستجو با ایمیل)
    /// </summary>
    public string? Email { get; private set; } // اضافه کردن private setter

    /// <summary>
    /// کد ملی کاربر (در صورت جستجو با کد ملی)
    /// </summary>
    public string? NationalCode { get; private set; } // اضافه کردن private setter

    /// <summary>
    /// شماره تلفن (در صورت جستجو با شماره تلفن)
    /// </summary>
    public string? PhoneNumber { get; private set; } // اضافه کردن private setter

    /// <summary>
    /// نوع جستجو
    /// </summary>
    public SearchType SearchType { get; }

    private UserNotFoundException(string message, SearchType searchType, string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
        SearchType = searchType;
        WithDetail(nameof(SearchType), searchType.ToString());
    }

    /// <summary>
    /// متد WithDetail با بازگشت نوع صحیح
    /// </summary>
    public new UserNotFoundException WithDetail(string key, object value)
    {
        base.WithDetail(key, value);
        return this;
    }

    /// <summary>
    /// ایجاد استثنا برای عدم یافتن کاربر با شناسه
    /// </summary>
    public static UserNotFoundException ForId(Guid userId)
    {
        var exception = new UserNotFoundException(
            $"کاربری با شناسه '{userId}' یافت نشد.",
            SearchType.ById,
            "AUTH.USER.NOT_FOUND_BY_ID"
        );
        exception.UserId = userId;
        return exception.WithDetail(nameof(UserId), userId);
    }

    /// <summary>
    /// ایجاد استثنا برای عدم یافتن کاربر با ایمیل
    /// </summary>
    public static UserNotFoundException ForEmail(string email)
    {
        var exception = new UserNotFoundException(
            $"کاربری با ایمیل '{email}' یافت نشد.",
            SearchType.ByEmail,
            "AUTH.USER.NOT_FOUND_BY_EMAIL"
        );
        exception.Email = email;
        return exception.WithDetail(nameof(Email), email);
    }

    /// <summary>
    /// ایجاد استثنا برای عدم یافتن کاربر با کد ملی
    /// </summary>
    public static UserNotFoundException ForNationalCode(string nationalCode)
    {
        var exception = new UserNotFoundException(
            $"کاربری با کد ملی '{nationalCode}' یافت نشد.",
            SearchType.ByNationalCode,
            "AUTH.USER.NOT_FOUND_BY_NATIONAL_CODE"
        );
        exception.NationalCode = nationalCode;
        return exception.WithDetail(nameof(NationalCode), nationalCode);
    }

    /// <summary>
    /// ایجاد استثنا برای عدم یافتن کاربر با شماره تلفن
    /// </summary>
    public static UserNotFoundException ForPhoneNumber(string phoneNumber)
    {
        var exception = new UserNotFoundException(
            $"کاربری با شماره تلفن '{phoneNumber}' یافت نشد.",
            SearchType.ByPhoneNumber,
            "AUTH.USER.NOT_FOUND_BY_PHONE"
        );
        exception.PhoneNumber = phoneNumber;
        return exception.WithDetail(nameof(PhoneNumber), phoneNumber);
    }

    /// <summary>
    /// ایجاد استثنا برای عدم یافتن کاربر با معیار ترکیبی
    /// </summary>
    public static UserNotFoundException ForCriteria(string criteria)
    {
        var exception = new UserNotFoundException(
            $"کاربری با معیار '{criteria}' یافت نشد.",
            SearchType.ByCriteria,
            "AUTH.USER.NOT_FOUND_BY_CRITERIA"
        );
        return exception.WithDetail("Criteria", criteria);
    }

    /// <summary>
    /// ایجاد استثنا عمومی
    /// </summary>
    public static UserNotFoundException General()
    {
        return new UserNotFoundException(
            "کاربر مورد نظر یافت نشد.",
            SearchType.General
        );
    }
}

/// <summary>
/// نوع جستجوی انجام شده
/// </summary>
public enum SearchType
{
    General,
    ById,
    ByEmail,
    ByNationalCode,
    ByPhoneNumber,
    ByCriteria
}
