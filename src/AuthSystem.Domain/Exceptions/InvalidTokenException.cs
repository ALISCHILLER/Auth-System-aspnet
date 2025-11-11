using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای توکن امنیتی نامعتبر
/// این استثنا زمانی رخ می‌دهد که توکن امنیتی از قوانین سیستم پیروی نکند
/// </summary>
public class InvalidTokenException : DomainException
{
    /// <summary>
    /// نوع توکن
    /// </summary>
    public string TokenType { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidToken";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidTokenException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidTokenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با نوع توکن و پیام خطا
    /// </summary>
    public InvalidTokenException(string tokenType, string message)
        : this(message)
    {
        TokenType = tokenType;
    }

    /// <summary>
    /// ایجاد استثنا برای توکن خالی
    /// </summary>
    public static InvalidTokenException ForEmptyToken()
    {
        return new InvalidTokenException("توکن نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای طول نامناسب توکن
    /// </summary>
    public static InvalidTokenException ForInvalidLength(int minLength, int maxLength)
    {
        return new InvalidTokenException($"طول توکن باید بین {minLength} و {maxLength} کاراکتر باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای فرمت نامناسب توکن
    /// </summary>
    public static InvalidTokenException ForInvalidFormat()
    {
        return new InvalidTokenException("فرمت توکن نامعتبر است");
    }

    /// <summary>
    /// ایجاد استثنا برای توکن منقضی شده
    /// </summary>
    public static InvalidTokenException ForExpiredToken()
    {
        return new InvalidTokenException("توکن منقضی شده است");
    }

    /// <summary>
    /// ایجاد استثنا برای توکن استفاده شده
    /// </summary>
    public static InvalidTokenException ForAlreadyUsedToken()
    {
        return new InvalidTokenException("توکن قبلاً استفاده شده است");
    }

    /// <summary>
    /// ایجاد استثنا برای توکن مربوط به نوع دیگر
    /// </summary>
    public static InvalidTokenException ForIncorrectTokenType(string expectedType, string actualType)
    {
        return new InvalidTokenException($"توکن نامعتبر است. نوع مورد انتظار: {expectedType}, نوع واقعی: {actualType}");
    }
}