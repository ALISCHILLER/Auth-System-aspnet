using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Common.Entities;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object برای کلید محرمانه احراز هویت دو عاملی
/// </summary>
public sealed class TwoFactorSecretKey : ValueObject
{
    /// <summary>
    /// طول کلید محرمانه (بایت)
    /// </summary>
    private const int SecretKeyLength = 20;

    /// <summary>
    /// نام صادرکننده پیش‌فرض
    /// </summary>
    private const string DefaultIssuer = "AuthSystem";

    /// <summary>
    /// کلید محرمانه (Base32 encoded)
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// نام صادرکننده (برای نمایش در Google Authenticator)
    /// </summary>
    public string Issuer { get; }

    /// <summary>
    /// تاریخ ایجاد کلید
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// آیا کلید فعال است
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// تاریخ آخرین استفاده
    /// </summary>
    public DateTime? LastUsedAt { get; private set; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private TwoFactorSecretKey(string value, string issuer, bool isActive = false)
    {
        Value = value;
        Issuer = issuer;
        CreatedAt = DateTime.UtcNow;
        IsActive = isActive;
    }

    /// <summary>
    /// تولید کلید محرمانه جدید
    /// </summary>
    public static TwoFactorSecretKey Generate(string issuer = DefaultIssuer)
    {
        if (string.IsNullOrWhiteSpace(issuer))
            issuer = DefaultIssuer;

        var secretKey = GenerateSecretKey();
        return new TwoFactorSecretKey(secretKey, issuer);
    }

    /// <summary>
    /// بازیابی از مقدار موجود
    /// </summary>
    public static TwoFactorSecretKey CreateFromExisting(
        string value,
        string issuer,
        bool isActive,
        DateTime? lastUsedAt = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("کلید محرمانه نمی‌تواند خالی باشد");

        if (!IsValidBase32(value))
            throw new InvalidTwoFactorSecretKeyException("فرمت کلید محرمانه نامعتبر است");

        var key = new TwoFactorSecretKey(value, issuer, isActive)
        {
            LastUsedAt = lastUsedAt
        };

        return key;
    }

    /// <summary>
    /// تولید کلید محرمانه تصادفی
    /// </summary>
    private static string GenerateSecretKey()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[SecretKeyLength];
        rng.GetBytes(bytes);
        return Base32Encode(bytes);
    }

    /// <summary>
    /// رمزنگاری Base32
    /// </summary>
    private static string Base32Encode(byte[] data)
    {
        const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var result = new StringBuilder((data.Length + 4) / 5 * 8);

        for (int i = 0; i < data.Length; i += 5)
        {
            var chunk = new byte[5];
            var chunkLength = Math.Min(5, data.Length - i);
            Array.Copy(data, i, chunk, 0, chunkLength);

            result.Append(base32Chars[(chunk[0] >> 3) & 0x1F]);
            result.Append(base32Chars[((chunk[0] << 2) | (chunk[1] >> 6)) & 0x1F]);

            if (chunkLength > 1)
                result.Append(base32Chars[(chunk[1] >> 1) & 0x1F]);

            if (chunkLength > 1)
                result.Append(base32Chars[((chunk[1] << 4) | (chunk[2] >> 4)) & 0x1F]);

            if (chunkLength > 2)
                result.Append(base32Chars[((chunk[2] << 1) | (chunk[3] >> 7)) & 0x1F]);

            if (chunkLength > 3)
                result.Append(base32Chars[(chunk[3] >> 2) & 0x1F]);

            if (chunkLength > 3)
                result.Append(base32Chars[((chunk[3] << 3) | (chunk[4] >> 5)) & 0x1F]);

            if (chunkLength > 4)
                result.Append(base32Chars[chunk[4] & 0x1F]);
        }

        return result.ToString();
    }

    /// <summary>
    /// بررسی معتبر بودن Base32
    /// </summary>
    private static bool IsValidBase32(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        foreach (char c in value)
        {
            if (!base32Chars.Contains(c))
                return false;
        }

        return true;
    }

    /// <summary>
    /// تولید URI برای QR Code
    /// </summary>
    public string GenerateUri(string userIdentifier)
    {
        if (string.IsNullOrWhiteSpace(userIdentifier))
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد");

        // فرمت: otpauth://totp/Issuer:user@example.com?secret=SECRET&issuer=Issuer
        return $"otpauth://totp/{Uri.EscapeDataString(Issuer)}:{Uri.EscapeDataString(userIdentifier)}" +
               $"?secret={Value}&issuer={Uri.EscapeDataString(Issuer)}";
    }

    /// <summary>
    /// فعال‌سازی کلید
    /// </summary>
    public TwoFactorSecretKey Activate()
    {
        var activated = new TwoFactorSecretKey(Value, Issuer, true)
        {
            LastUsedAt = DateTime.UtcNow
        };

        return activated;
    }

    /// <summary>
    /// غیرفعال‌سازی کلید
    /// </summary>
    public TwoFactorSecretKey Deactivate()
    {
        var deactivated = new TwoFactorSecretKey(Value, Issuer, false)
        {
            LastUsedAt = LastUsedAt
        };

        return deactivated;
    }

    /// <summary>
    /// ثبت استفاده از کلید
    /// </summary>
    public TwoFactorSecretKey RecordUsage()
    {
        var updated = new TwoFactorSecretKey(Value, Issuer, IsActive)
        {
            LastUsedAt = DateTime.UtcNow
        };

        return updated;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Issuer;
    }

    public override string ToString() => "[2FA Secret Key]";
}