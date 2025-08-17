using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای توکن نامعتبر
/// </summary>
[Serializable]
public sealed class InvalidTokenException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.TOKEN.INVALID";

    /// <summary>
    /// مقدار توکن نامعتبر (برای امنیت فقط بخشی از آن)
    /// </summary>
    public string? TokenPreview { get; private set; }

    /// <summary>
    /// نوع توکن
    /// </summary>
    public string? TokenType { get; private set; }

    /// <summary>
    /// دلیل نامعتبر بودن
    /// </summary>
    public TokenInvalidReason Reason { get; private set; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private InvalidTokenException(
        string message,
        string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
        Reason = TokenInvalidReason.InvalidFormat;
    }

    private InvalidTokenException(
        string message,
        Exception innerException,
        string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode, innerException)
    {
        Reason = TokenInvalidReason.InvalidFormat;
    }

    /// <summary>
    /// متد WithDetail با بازگشت نوع صحیح
    /// </summary>
    public new InvalidTokenException WithDetail(string key, object value)
    {
        base.WithDetail(key, value);
        return this;
    }

    /// <summary>
    /// سازنده عمومی برای پیام دلخواه
    /// </summary>
    public InvalidTokenException(string message) : this(message, DEFAULT_ERROR_CODE) { }

    /// <summary>
    /// ایجاد استثنا برای توکن خالی
    /// </summary>
    public static InvalidTokenException ForEmptyToken()
    {
        return new InvalidTokenException(
            "توکن نمی‌تواند خالی باشد.",
            "AUTH.TOKEN.EMPTY"
        )
        {
            Reason = TokenInvalidReason.Empty
        };
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت نامعتبر
    /// </summary>
    public static InvalidTokenException ForInvalidFormat(string? tokenPreview = null)
    {
        return new InvalidTokenException(
            "فرمت توکن نامعتبر است.",
            "AUTH.TOKEN.INVALID_FORMAT"
        )
        {
            TokenPreview = GetSafeTokenPreview(tokenPreview),
            Reason = TokenInvalidReason.InvalidFormat
        }
        .WithDetail(nameof(TokenPreview), GetSafeTokenPreview(tokenPreview));
    }

    /// <summary>
    /// ایجاد استثنا برای توکن منقضی شده
    /// </summary>
    public static InvalidTokenException ForExpiredToken(string? tokenType = null, DateTime? expiredAt = null)
    {
        var message = tokenType != null
            ? $"توکن {tokenType} منقضی شده است."
            : "توکن منقضی شده است.";

        var exception = new InvalidTokenException(
            message,
            "AUTH.TOKEN.EXPIRED"
        )
        {
            TokenType = tokenType,
            Reason = TokenInvalidReason.Expired
        };

        if (tokenType != null)
            exception.WithDetail(nameof(TokenType), tokenType);

        if (expiredAt.HasValue)
            exception.WithDetail("ExpiredAt", expiredAt.Value);

        return exception;
    }

    /// <summary>
    /// ایجاد استثنا برای توکن استفاده شده
    /// </summary>
    public static InvalidTokenException ForUsedToken(string? tokenType = null)
    {
        var message = tokenType != null
            ? $"توکن {tokenType} قبلاً استفاده شده است."
            : "توکن قبلاً استفاده شده است.";

        return new InvalidTokenException(
            message,
            "AUTH.TOKEN.ALREADY_USED"
        )
        {
            TokenType = tokenType,
            Reason = TokenInvalidReason.AlreadyUsed
        }
        .WithDetail(nameof(TokenType), tokenType ?? "Unknown");
    }

    /// <summary>
    /// ایجاد استثنا برای توکن نامعتبر
    /// </summary>
    public static InvalidTokenException ForInvalidToken(string? reason = null)
    {
        var message = !string.IsNullOrWhiteSpace(reason)
            ? $"توکن نامعتبر است: {reason}"
            : "توکن نامعتبر است.";

        return new InvalidTokenException(
            message,
            "AUTH.TOKEN.INVALID"
        )
        {
            Reason = TokenInvalidReason.Invalid
        }
        .WithDetail("InvalidReason", reason ?? "Unknown");
    }

    /// <summary>
    /// ایجاد استثنا برای طول نامعتبر
    /// </summary>
    public static InvalidTokenException ForInvalidLength(int actualLength, int minLength, int maxLength)
    {
        return new InvalidTokenException(
            $"طول توکن ({actualLength}) باید بین {minLength} و {maxLength} کاراکتر باشد.",
            "AUTH.TOKEN.INVALID_LENGTH"
        )
        {
            Reason = TokenInvalidReason.InvalidLength
        }
        .WithDetail("ActualLength", actualLength)
        .WithDetail("MinLength", minLength)
        .WithDetail("MaxLength", maxLength);
    }

    /// <summary>
    /// ایجاد استثنا برای نوع توکن نامعتبر
    /// </summary>
    public static InvalidTokenException ForInvalidType(string expectedType, string actualType)
    {
        return new InvalidTokenException(
            $"نوع توکن نامعتبر است. انتظار: {expectedType}، واقعی: {actualType}",
            "AUTH.TOKEN.INVALID_TYPE"
        )
        {
            TokenType = actualType,
            Reason = TokenInvalidReason.InvalidType
        }
        .WithDetail("ExpectedType", expectedType)
        .WithDetail("ActualType", actualType);
    }

    /// <summary>
    /// ایجاد استثنا برای توکن غیرفعال
    /// </summary>
    public static InvalidTokenException ForDeactivatedToken(string? tokenType = null)
    {
        var message = tokenType != null
            ? $"توکن {tokenType} غیرفعال شده است."
            : "توکن غیرفعال شده است.";

        return new InvalidTokenException(
            message,
            "AUTH.TOKEN.DEACTIVATED"
        )
        {
            TokenType = tokenType,
            Reason = TokenInvalidReason.Deactivated
        }
        .WithDetail(nameof(TokenType), tokenType ?? "Unknown");
    }

    /// <summary>
    /// دریافت پیش‌نمایش امن از توکن (فقط چند کاراکتر اول و آخر)
    /// </summary>
    private static string GetSafeTokenPreview(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return "***";

        if (token.Length <= 10)
            return "***";

        return $"{token.Substring(0, 4)}...{token.Substring(token.Length - 4)}";
    }
}

/// <summary>
/// دلایل نامعتبر بودن توکن
/// </summary>
public enum TokenInvalidReason
{
    Empty,
    InvalidFormat,
    InvalidLength,
    Expired,
    AlreadyUsed,
    Invalid,
    InvalidType,
    Deactivated,
    NotFound,
    Revoked
}
