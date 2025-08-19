using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object برای توکن‌های امنیتی
/// </summary>
public sealed class TokenValue : ValueObject
{
    /// <summary>
    /// حداقل طول توکن
    /// </summary>
    private const int MinLength = 32;

    /// <summary>
    /// حداکثر طول توکن
    /// </summary>
    private const int MaxLength = 512;

    /// <summary>
    /// الگوی توکن معتبر (Base64 URL-safe)
    /// </summary>
    private static readonly Regex TokenPattern = new(
        @"^[A-Za-z0-9_-]+$",
        RegexOptions.Compiled);

    /// <summary>
    /// مقدار توکن
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// نوع توکن
    /// </summary>
    public TokenType Type { get; }

    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// تاریخ انقضا (اختیاری)
    /// </summary>
    public DateTime? ExpiresAt { get; }

    /// <summary>
    /// آیا توکن منقضی شده است
    /// </summary>
    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;

    /// <summary>
    /// مدت زمان باقی‌مانده تا انقضا
    /// </summary>
    public TimeSpan? TimeToExpiry => ExpiresAt.HasValue && !IsExpired
        ? ExpiresAt.Value - DateTime.UtcNow
        : null;

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private TokenValue(string value, TokenType type, DateTime? expiresAt = null)
    {
        Value = value;
        Type = type;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
    }

    /// <summary>
    /// ایجاد توکن از مقدار موجود
    /// </summary>
    public static TokenValue Create(string value, TokenType type, DateTime? expiresAt = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidTokenException("توکن نمی‌تواند خالی باشد");

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new InvalidTokenException($"طول توکن باید بین {MinLength} و {MaxLength} کاراکتر باشد");

        if (!TokenPattern.IsMatch(value))
            throw new InvalidTokenException("فرمت توکن نامعتبر است");

        if (expiresAt.HasValue && expiresAt.Value <= DateTime.UtcNow)
            throw new InvalidTokenException("زمان انقضای توکن باید در آینده باشد");

        return new TokenValue(value, type, expiresAt);
    }

    /// <summary>
    /// تولید توکن تصادفی جدید
    /// </summary>
    public static TokenValue Generate(TokenType type, int length = 64, TimeSpan? validity = null)
    {
        if (length < MinLength || length > MaxLength)
            throw new ArgumentException($"طول توکن باید بین {MinLength} و {MaxLength} کاراکتر باشد");

        var token = GenerateSecureToken(length);
        var expiresAt = validity.HasValue ? DateTime.UtcNow.Add(validity.Value) : (DateTime?)null;
        return new TokenValue(token, type, expiresAt);
    }

    /// <summary>
    /// تولید توکن امن
    /// </summary>
    private static string GenerateSecureToken(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length * 3 / 4]; // برای تولید طول مناسب Base64
        rng.GetBytes(bytes);
        // تبدیل به Base64 URL-safe
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=')
            .Substring(0, length);
    }

    /// <summary>
    /// هش کردن توکن (برای ذخیره در دیتابیس)
    /// </summary>
    public string GetHash()
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Value));
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// بررسی اعتبار توکن
    /// </summary>
    public bool IsValid()
    {
        return !IsExpired;
    }

    /// <summary>
    /// ایجاد نسخه منقضی شده از توکن
    /// </summary>
    public TokenValue Expire()
    {
        return new TokenValue(Value, Type, DateTime.UtcNow.AddSeconds(-1));
    }

    /// <summary>
    /// تمدید اعتبار توکن
    /// </summary>
    public TokenValue Extend(TimeSpan extension)
    {
        var newExpiry = ExpiresAt.HasValue
            ? ExpiresAt.Value.Add(extension)
            : DateTime.UtcNow.Add(extension);
        return new TokenValue(Value, Type, newExpiry);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
    }

    public override string ToString() => $"[{Type} Token: ***]";
}