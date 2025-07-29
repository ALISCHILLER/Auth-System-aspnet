using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای زمانی که توکن تازه‌سازی نامعتبر است
/// </summary>
public class InvalidRefreshTokenException : DomainException
{
    /// <summary>
    /// مقدار توکن تازه‌سازی
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// سازنده با مقدار توکن
    /// </summary>
    /// <param name="token">مقدار توکن</param>
    public InvalidRefreshTokenException(string token)
        : base($"توکن تازه‌سازی {token} نامعتبر است")
    {
        Token = token;
    }

    /// <summary>
    /// سازنده با مقدار توکن و پیام خطا
    /// </summary>
    /// <param name="token">مقدار توکن</param>
    /// <param name="message">پیام خطا</param>
    public InvalidRefreshTokenException(string token, string message)
        : base(message)
    {
        Token = token;
    }
}