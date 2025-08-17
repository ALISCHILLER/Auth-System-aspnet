using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای ایمیل نامعتبر
/// </summary>
[Serializable]
public sealed class InvalidEmailException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.EMAIL.INVALID";

    /// <summary>
    /// آدرس ایمیل نامعتبر
    /// </summary>
    public string? InvalidEmail { get; private set; }

    /// <summary>
    /// دلیل نامعتبر بودن
    /// </summary>
    public EmailInvalidReason Reason { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private InvalidEmailException(
        string message,
        EmailInvalidReason reason = EmailInvalidReason.InvalidFormat,
        string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
        Reason = reason;
        WithDetail(nameof(Reason), reason.ToString());
    }

    /// <summary>
    /// متد WithDetail با بازگشت نوع صحیح
    /// </summary>
    public new InvalidEmailException WithDetail(string key, object value)
    {
        base.WithDetail(key, value);
        return this;
    }

    /// <summary>
    /// تنظیم ایمیل نامعتبر
    /// </summary>
    private InvalidEmailException WithInvalidEmail(string email)
    {
        InvalidEmail = email;
        return WithDetail(nameof(InvalidEmail), email);
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت نامعتبر ایمیل
    /// </summary>
    public static InvalidEmailException ForInvalidFormat(string email)
    {
        var exception = new InvalidEmailException(
            $"فرمت ایمیل '{email}' نامعتبر است.",
            EmailInvalidReason.InvalidFormat
        );
        return exception.WithInvalidEmail(email);
    }

    /// <summary>
    /// ایجاد استثنا برای ایمیل خالی
    /// </summary>
    public static InvalidEmailException ForEmptyEmail()
    {
        return new InvalidEmailException(
            "آدرس ایمیل نمی‌تواند خالی باشد.",
            EmailInvalidReason.Empty,
            "AUTH.EMAIL.EMPTY"
        );
    }

    /// <summary>
    /// ایجاد استثنا برای ایمیل تکراری
    /// </summary>
    public static InvalidEmailException ForDuplicateEmail(string email)
    {
        var exception = new InvalidEmailException(
            $"ایمیل '{email}' قبلاً ثبت شده است.",
            EmailInvalidReason.AlreadyExists,
            "AUTH.EMAIL.DUPLICATE"
        );
        return exception.WithInvalidEmail(email);
    }

    /// <summary>
    /// ایجاد استثنا برای دامنه غیرمجاز
    /// </summary>
    public static InvalidEmailException ForBlockedDomain(string email, string domain)
    {
        var exception = new InvalidEmailException(
            $"دامنه '{domain}' برای ثبت‌نام مجاز نیست.",
            EmailInvalidReason.BlockedDomain,
            "AUTH.EMAIL.BLOCKED_DOMAIN"
        );
        return exception
            .WithInvalidEmail(email)
            .WithDetail("Domain", domain);
    }

    /// <summary>
    /// ایجاد استثنا برای ایمیل موقت
    /// </summary>
    public static InvalidEmailException ForDisposableEmail(string email)
    {
        var exception = new InvalidEmailException(
            "استفاده از ایمیل‌های موقت مجاز نیست.",
            EmailInvalidReason.DisposableEmail,
            "AUTH.EMAIL.DISPOSABLE"
        );
        return exception.WithInvalidEmail(email);
    }

    /// <summary>
    /// ایجاد استثنا برای طول نامعتبر
    /// </summary>
    public static InvalidEmailException ForInvalidLength(string email, int maxLength = 254)
    {
        var exception = new InvalidEmailException(
            $"طول ایمیل نباید بیشتر از {maxLength} کاراکتر باشد.",
            EmailInvalidReason.TooLong,
            "AUTH.EMAIL.TOO_LONG"
        );
        return exception
            .WithInvalidEmail(email)
            .WithDetail("MaxLength", maxLength)
            .WithDetail("ActualLength", email.Length);
    }

    /// <summary>
    /// ایجاد استثنا عمومی برای ایمیل نامعتبر
    /// </summary>
    public static InvalidEmailException ForInvalidEmail(string email)
    {
        var exception = new InvalidEmailException(
            $"ایمیل '{email}' نامعتبر است."
        );
        return exception.WithInvalidEmail(email);
    }
}

/// <summary>
/// دلایل نامعتبر بودن ایمیل
/// </summary>
public enum EmailInvalidReason
{
    InvalidFormat,
    Empty,
    AlreadyExists,
    BlockedDomain,
    DisposableEmail,
    TooLong
}
