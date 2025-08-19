using System;
using System.Collections.Generic;
using System.Security.Cryptography; // این خط را اضافه کردم
using System.Text;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums; // این خط را اضافه کردم
using AuthSystem.Domain.Exceptions;
using HashAlgorithm = AuthSystem.Domain.Enums.HashAlgorithm;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object برای نگهداری هش رمز عبور
/// این کلاس مسئول مدیریت ایمن هش رمز عبور است
/// </summary>
public sealed class PasswordHash : ValueObject
{
    /// <summary>
    /// طول هش مورد انتظار (برای BCrypt)
    /// </summary>
    private const int ExpectedHashLength = 60;

    /// <summary>
    /// مقدار هش شده رمز عبور
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// الگوریتم استفاده شده
    /// </summary>
    public HashAlgorithm Algorithm { get; } // از AuthSystem.Domain.Enums.HashAlgorithm استفاده می‌شود

    /// <summary>
    /// تاریخ ایجاد هش (برای بررسی نیاز به بازنشانی)
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// آیا این هش نیاز به به‌روزرسانی دارد (قدیمی است)
    /// </summary>
    public bool NeedsRehash => CreatedAt < DateTime.UtcNow.AddMonths(-6);

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private PasswordHash(string value, HashAlgorithm algorithm, DateTime? createdAt = null)
    {
        Value = value;
        Algorithm = algorithm;
        CreatedAt = createdAt ?? DateTime.UtcNow;
    }

    /// <summary>
    /// ایجاد هش از رمز عبور خام
    /// </summary>
    public static PasswordHash CreateFromPlainText(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new InvalidPasswordException("رمز عبور نمی‌تواند خالی باشد");

        // استفاده از BCrypt به عنوان پیش‌فرض
        // در عمل باید از کتابخانه BCrypt.Net استفاده کنید
        var hash = HashPassword(plainPassword);
        return new PasswordHash(hash, HashAlgorithm.BCrypt);
    }

    /// <summary>
    /// ایجاد از هش موجود (برای بازیابی از دیتابیس)
    /// </summary>
    public static PasswordHash CreateFromHash(string hash, DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentNullException(nameof(hash));

        var algorithm = DetectAlgorithm(hash);
        return new PasswordHash(hash, algorithm, createdAt);
    }

    /// <summary>
    /// بررسی تطابق رمز عبور با هش
    /// </summary>
    public bool Verify(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            return false;

        return VerifyPassword(plainPassword, Value);
    }

    /// <summary>
    /// ایجاد هش جدید در صورت نیاز
    /// </summary>
    public PasswordHash RehashIfNeeded(string plainPassword)
    {
        if (!NeedsRehash)
            return this;

        return CreateFromPlainText(plainPassword);
    }

    /// <summary>
    /// شبیه‌سازی هش کردن رمز عبور (در عمل از BCrypt استفاده کنید)
    /// </summary>
    private static string HashPassword(string password)
    {
        // این فقط یک نمونه است - در عمل از BCrypt.Net استفاده کنید
        // return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        using var sha256 = SHA256.Create(); // SHA256 اکنون شناخته می‌شود
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt"));
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// شبیه‌سازی بررسی رمز عبور (در عمل از BCrypt استفاده کنید)
    /// </summary>
    private static bool VerifyPassword(string password, string hash)
    {
        // در عمل: return BCrypt.Net.BCrypt.Verify(password, hash);
        var testHash = HashPassword(password);
        return testHash == hash;
    }

    /// <summary>
    /// تشخیص الگوریتم از روی فرمت هش
    /// </summary>
    private static HashAlgorithm DetectAlgorithm(string hash)
    {
        if (hash.StartsWith("$2a$") || hash.StartsWith("$2b$") || hash.StartsWith("$2y$"))
            return HashAlgorithm.BCrypt;

        if (hash.StartsWith("$argon2"))
            return HashAlgorithm.Argon2;

        return HashAlgorithm.PBKDF2;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => $"[PROTECTED HASH - {Algorithm}]";
}