using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای رمز عبور نامعتبر
/// </summary>
[Serializable]
public sealed class InvalidPasswordException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.PASSWORD.INVALID";

    /// <summary>
    /// نوع خطای اعتبارسنجی
    /// </summary>
    public PasswordValidationError ValidationError { get; }

    /// <summary>
    /// طول فعلی رمز عبور (در صورت خطای طول)
    /// </summary>
    public int? ActualLength { get; private set; }

    /// <summary>
    /// حداقل طول مورد نیاز
    /// </summary>
    public int? MinLength { get; private set; }

    /// <summary>
    /// حداکثر طول مجاز
    /// </summary>
    public int? MaxLength { get; private set; }

    private InvalidPasswordException(
        string message,
        PasswordValidationError validationError = PasswordValidationError.InvalidFormat,
        string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
        ValidationError = validationError;
        WithDetail(nameof(ValidationError), validationError.ToString());
    }

    /// <summary>
    /// متد WithDetail با بازگشت نوع صحیح
    /// </summary>
    public new InvalidPasswordException WithDetail(string key, object value)
    {
        base.WithDetail(key, value);
        return this;
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور خالی
    /// </summary>
    public static InvalidPasswordException ForEmptyPassword()
    {
        return new InvalidPasswordException(
            "رمز عبور نمی‌تواند خالی باشد.",
            PasswordValidationError.Empty,
            "AUTH.PASSWORD.EMPTY"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای طول نامعتبر
    /// </summary>
    public static InvalidPasswordException ForInvalidLength(int actualLength, int minLength = 8, int maxLength = 128)
    {
        var message = actualLength < minLength
            ? $"رمز عبور باید حداقل {minLength} کاراکتر باشد."
            : $"رمز عبور نباید بیشتر از {maxLength} کاراکتر باشد.";

        var exception = new InvalidPasswordException(
            message,
            actualLength < minLength ? PasswordValidationError.TooShort : PasswordValidationError.TooLong,
            actualLength < minLength ? "AUTH.PASSWORD.TOO_SHORT" : "AUTH.PASSWORD.TOO_LONG"
        );

        exception.ActualLength = actualLength;
        exception.MinLength = minLength;
        exception.MaxLength = maxLength;

        return exception
            .WithDetail(nameof(ActualLength), actualLength)
            .WithDetail(nameof(MinLength), minLength)
            .WithDetail(nameof(MaxLength), maxLength);
    }

    /// <summary>
    /// ایجاد استثنا برای عدم رعایت پیچیدگی
    /// </summary>
    public static InvalidPasswordException ForWeakPassword()
    {
        return new InvalidPasswordException(
            "رمز عبور باید حداقل شامل 3 مورد از موارد زیر باشد: حرف بزرگ، حرف کوچک، عدد، کاراکتر خاص.",
            PasswordValidationError.NotComplex,
            "AUTH.PASSWORD.WEAK"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور نامعتبر عمومی
    /// </summary>
    public static InvalidPasswordException ForInvalidPassword()
    {
        return new InvalidPasswordException(
            "رمز عبور نامعتبر است."
        );
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور رایج
    /// </summary>
    public static InvalidPasswordException ForCommonPassword()
    {
        return new InvalidPasswordException(
            "این رمز عبور بسیار رایج است و امنیت کافی ندارد.",
            PasswordValidationError.TooCommon,
            "AUTH.PASSWORD.COMMON"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای شباهت به اطلاعات کاربر
    /// </summary>
    public static InvalidPasswordException ForSimilarToUserInfo()
    {
        return new InvalidPasswordException(
            "رمز عبور نباید شبیه به نام کاربری یا سایر اطلاعات شخصی باشد.",
            PasswordValidationError.SimilarToUserInfo,
            "AUTH.PASSWORD.SIMILAR_TO_USER_INFO"
        );
    }
    /// <summary>
    /// ایجاد استثنا برای رمز عبور فعلی نادرست
    /// </summary>
    public static InvalidPasswordException ForIncorrectCurrentPassword()
    {
        return new InvalidPasswordException(
            "رمز عبور فعلی نادرست است.",
            PasswordValidationError.InvalidFormat,
            "AUTH.PASSWORD.INCORRECT_CURRENT"
        );
    }


}

/// <summary>
/// انواع خطاهای اعتبارسنجی رمز عبور
/// </summary>
public enum PasswordValidationError
{
    InvalidFormat,
    Empty,
    TooShort,
    TooLong,
    NotComplex,
    TooCommon,
    SimilarToUserInfo
}
