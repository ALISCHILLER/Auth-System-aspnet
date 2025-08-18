// File: AuthSystem.Domain/Exceptions/InvalidTokenException.cs
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Enums;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای توکن امنیتی نامعتبر
/// - هنگام اعتبارسنجی توکن‌های امنیتی رخ می‌دهد
/// - شامل جزئیات خطا برای ارائه پیام مناسب به کاربر
/// </summary>
public class InvalidTokenException : DomainException
{
    /// <summary>
    /// نوع توکن
    /// </summary>
    public TokenType? TokenType { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private InvalidTokenException(string message, string errorCode, TokenType? tokenType = null)
        : base(message, errorCode)
    {
        TokenType = tokenType;
        if (tokenType.HasValue)
            Data.Add("TokenType", tokenType.Value.ToString());
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای توکن خالی
    /// </summary>
    public static InvalidTokenException Empty()
        => new InvalidTokenException(
            "توکن نمی‌تواند خالی باشد",
            "TOKEN_EMPTY");

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای فرمت نامعتبر توکن
    /// </summary>
    public static InvalidTokenException InvalidFormat(string reason)
    {
        var ex = new InvalidTokenException(
            $"فرمت توکن نامعتبر است: {reason}",
            "TOKEN_INVALID_FORMAT");

        ex.Data.Add("Reason", reason);
        return ex;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای توکن منقضی شده
    /// </summary>
    public static InvalidTokenException Expired(DateTime expirationTime)
        => new InvalidTokenException(
            $"توکن منقضی شده است و تاریخ انقضا آن {expirationTime:yyyy/MM/dd HH:mm:ss} بوده است",
            "TOKEN_EXPIRED")
        {
            Data = { ["ExpirationTime"] = expirationTime }
        };

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای توکن غیرفعال
    /// </summary>
    public static InvalidTokenException NotActive(string token)
        => new InvalidTokenException(
            $"توکن معتبر نیست",
            "TOKEN_NOT_ACTIVE")
        {
            Data = { ["Token"] = token }
        };
}

