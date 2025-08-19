using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Common.Entities;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object برای کد تایید یکبار مصرف (OTP)
/// </summary>
public sealed class OtpCode : ValueObject
{
    /// <summary>
    /// طول پیش‌فرض کد
    /// </summary>
    private const int DefaultLength = 6;

    /// <summary>
    /// حداقل طول کد
    /// </summary>
    private const int MinLength = 4;

    /// <summary>
    /// حداکثر طول کد
    /// </summary>
    private const int MaxLength = 10;

    /// <summary>
    /// مدت اعتبار پیش‌فرض (دقیقه)
    /// </summary>
    private const int DefaultValidityMinutes = 10;

    /// <summary>
    /// حداکثر تعداد تلاش
    /// </summary>
    private const int MaxAttempts = 3;

    /// <summary>
    /// مقدار کد
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// نوع کد تایید
    /// </summary>
    public VerificationCodeType Type { get; }

    /// <summary>
    /// زمان ایجاد
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// زمان انقضا
    /// </summary>
    public DateTime ExpiresAt { get; }

    /// <summary>
    /// تعداد تلاش‌های انجام شده
    /// </summary>
    public int AttemptCount { get; private set; }

    /// <summary>
    /// آیا استفاده شده است
    /// </summary>
    public bool IsUsed { get; private set; }

    /// <summary>
    /// زمان استفاده
    /// </summary>
    public DateTime? UsedAt { get; private set; }

    /// <summary>
    /// آیا منقضی شده است
    /// </summary>
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    /// <summary>
    /// آیا قابل استفاده است
    /// </summary>
    public bool IsValid => !IsExpired && !IsUsed && AttemptCount < MaxAttempts;

    /// <summary>
    /// تعداد تلاش‌های باقی‌مانده
    /// </summary>
    public int RemainingAttempts => Math.Max(0, MaxAttempts - AttemptCount);

    /// <summary>
    /// زمان باقی‌مانده تا انقضا
    /// </summary>
    public TimeSpan? TimeToExpiry => !IsExpired
        ? ExpiresAt - DateTime.UtcNow
        : null;

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private OtpCode(
        string value,
        VerificationCodeType type,
        DateTime expiresAt,
        int attemptCount = 0,
        bool isUsed = false,
        DateTime? usedAt = null)
    {
        Value = value;
        Type = type;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        AttemptCount = attemptCount;
        IsUsed = isUsed;
        UsedAt = usedAt;
    }

    /// <summary>
    /// تولید کد تایید عددی جدید
    /// </summary>
    public static OtpCode GenerateNumeric(
        VerificationCodeType type,
        int length = DefaultLength,
        int? validityMinutes = null)
    {
        ValidateLength(length);
        var code = GenerateNumericCode(length);
        var validity = validityMinutes ?? DefaultValidityMinutes;
        var expiresAt = DateTime.UtcNow.AddMinutes(validity);
        return new OtpCode(code, type, expiresAt);
    }

    /// <summary>
    /// تولید کد تایید الفبایی-عددی جدید
    /// </summary>
    public static OtpCode GenerateAlphanumeric(
        VerificationCodeType type,
        int length = DefaultLength,
        int? validityMinutes = null)
    {
        ValidateLength(length);
        var code = GenerateAlphanumericCode(length);
        var validity = validityMinutes ?? DefaultValidityMinutes;
        var expiresAt = DateTime.UtcNow.AddMinutes(validity);
        return new OtpCode(code, type, expiresAt);
    }

    /// <summary>
    /// ایجاد از کد موجود (برای بازیابی از دیتابیس)
    /// </summary>
    public static OtpCode CreateFromExisting(
        string value,
        VerificationCodeType type,
        DateTime createdAt,
        DateTime expiresAt,
        int attemptCount = 0,
        bool isUsed = false,
        DateTime? usedAt = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("کد تایید نمی‌تواند خالی باشد");

        return new OtpCode(value, type, expiresAt, attemptCount, isUsed, usedAt);
    }

    /// <summary>
    /// بررسی اعتبار کد
    /// </summary>
    public bool Verify(string code)
    {
        if (!IsValid)
            return false;

        // ثبت تلاش
        AttemptCount++;

        if (string.IsNullOrWhiteSpace(code))
            return false;

        // مقایسه بدون حساسیت به حروف بزرگ و کوچک
        var isMatch = Value.Equals(code, StringComparison.OrdinalIgnoreCase);

        if (isMatch)
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }

        return isMatch;
    }

    /// <summary>
    /// ثبت تلاش ناموفق
    /// </summary>
    public OtpCode RecordFailedAttempt()
    {
        return new OtpCode(
            Value, Type, ExpiresAt,
            AttemptCount + 1, IsUsed, UsedAt);
    }

    /// <summary>
    /// علامت‌گذاری به عنوان استفاده شده
    /// </summary>
    public OtpCode MarkAsUsed()
    {
        return new OtpCode(
            Value, Type, ExpiresAt,
            AttemptCount, true, DateTime.UtcNow);
    }

    /// <summary>
    /// بررسی معتبر بودن طول کد
    /// </summary>
    private static void ValidateLength(int length)
    {
        if (length < MinLength || length > MaxLength)
            throw new ArgumentException(
                $"طول کد باید بین {MinLength} و {MaxLength} کاراکتر باشد");
    }

    /// <summary>
    /// تولید کد عددی تصادفی
    /// </summary>
    private static string GenerateNumericCode(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var randomNumber = BitConverter.ToUInt32(bytes, 0);
        var code = (randomNumber % (uint)Math.Pow(10, length)).ToString();
        return code.PadLeft(length, '0');
    }

    /// <summary>
    /// تولید کد الفبایی-عددی تصادفی
    /// </summary>
    private static string GenerateAlphanumericCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        using var rng = RandomNumberGenerator.Create();
        var result = new char[length];
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[bytes[i] % chars.Length];
        }
        return new string(result);
    }

    /// <summary>
    /// دریافت کد با فرمت مناسب برای نمایش
    /// </summary>
    public string GetFormattedCode()
    {
        // برای کدهای 6 رقمی: 123-456
        if (Value.Length == 6)
            return $"{Value.Substring(0, 3)}-{Value.Substring(3, 3)}";

        // برای کدهای 8 رقمی: 1234-5678
        if (Value.Length == 8)
            return $"{Value.Substring(0, 4)}-{Value.Substring(4, 4)}";

        return Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
        yield return CreatedAt;
    }

    public override string ToString() => $"[{Type} OTP Code: ***]";
}