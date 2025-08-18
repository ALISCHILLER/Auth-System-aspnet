// File: AuthSystem.Domain/Exceptions/InvalidTwoFactorSecretKeyException.cs
using AuthSystem.Domain.Common.Exceptions;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای کلید محرمانه احراز هویت دو عاملی نامعتبر
/// - هنگام اعتبارسنجی کلید 2FA رخ می‌دهد
/// - شامل جزئیات خطا برای ارائه پیام مناسب به کاربر
/// </summary>
public class InvalidTwoFactorSecretKeyException : DomainException
{
    /// <summary>
    /// کلید محرمانه نامعتبر
    /// </summary>
    public string? SecretKey { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private InvalidTwoFactorSecretKeyException(string message, string errorCode, string? secretKey = null)
        : base(message, errorCode)
    {
        SecretKey = secretKey;
        if (secretKey != null)
            Data.Add("SecretKey", secretKey);
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای کلید خالی
    /// </summary>
    public static InvalidTwoFactorSecretKeyException Empty()
        => new InvalidTwoFactorSecretKeyException(
            "کلید محرمانه نمی‌تواند خالی باشد",
            "TWO_FACTOR_SECRET_KEY_EMPTY");

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای فرمت نامعتبر کلید
    /// </summary>
    public static InvalidTwoFactorSecretKeyException InvalidFormat(string secretKey, string reason)
    {
        var ex = new InvalidTwoFactorSecretKeyException(
            $"فرمت کلید محرمانه '{secretKey}' نامعتبر است: {reason}",
            "TWO_FACTOR_SECRET_KEY_INVALID_FORMAT",
            secretKey);

        ex.Data.Add("Reason", reason);
        return ex;
    }

    /// <summary>
    /// سازنده استاتیک برای ایجاد استثنا برای کلید غیرفعال
    /// </summary>
    public static InvalidTwoFactorSecretKeyException NotActive(string secretKey)
        => new InvalidTwoFactorSecretKeyException(
            $"کلید محرمانه '{secretKey}' غیرفعال است",
            "TWO_FACTOR_SECRET_KEY_NOT_ACTIVE",
            secretKey);
}