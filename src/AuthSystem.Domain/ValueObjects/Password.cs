using AuthSystem.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using AuthSystem.Domain.Common.Entities;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object رمز عبور با قابلیت هش کردن امن
/// </summary>
public sealed class Password : ValueObject
{
    // ثابت‌های امنیتی
    private const int SaltSize = 128 / 8; // 16 bytes
    private const int KeySize = 256 / 8;  // 32 bytes
    private const int Iterations = 100_000;
    private const char Delimiter = ';';

    /// <summary>
    /// رمز عبور به صورت Plain Text (فقط هنگام ایجاد)
    /// </summary>
    private readonly string? _plainTextValue;

    /// <summary>
    /// رمز عبور هش شده
    /// </summary>
    public string HashedValue { get; }

    /// <summary>
    /// آیا این رمز عبور هش شده است؟
    /// </summary>
    public bool IsHashed { get; }

    /// <summary>
    /// مقدار رمز عبور به صورت متن ساده (فقط برای اعتبارسنجی)
    /// </summary>
    public string Value => _plainTextValue ?? throw new InvalidOperationException("مقدار متن ساده رمز عبور پس از هش شدن در دسترس نیست");

    private Password(string value, bool isHashed)
    {
        if (isHashed)
        {
            HashedValue = value;
            _plainTextValue = null;
        }
        else
        {
            _plainTextValue = value;
            HashedValue = HashPassword(value);
        }
        IsHashed = true;
    }

    /// <summary>
    /// ایجاد رمز عبور جدید با اعتبارسنجی
    /// </summary>
    public static Password Create(string plainTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            throw InvalidPasswordException.ForEmptyPassword();

        if (plainTextPassword.Length < 8 || plainTextPassword.Length > 128)
            throw InvalidPasswordException.ForInvalidLength(plainTextPassword.Length);

        if (!IsValidPasswordComplexity(plainTextPassword))
            throw InvalidPasswordException.ForWeakPassword();

        return new Password(plainTextPassword, false);
    }

    /// <summary>
    /// بازیابی رمز عبور از مقدار هش شده (از دیتابیس)
    /// </summary>
    public static Password CreateFromHash(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentNullException(nameof(hashedPassword));

        return new Password(hashedPassword, true);
    }

    /// <summary>
    /// بررسی تطابق رمز عبور
    /// </summary>
    public bool Verify(string plainTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            return false;

        return VerifyPassword(plainTextPassword, HashedValue);
    }

    /// <summary>
    /// هش کردن رمز عبور با PBKDF2
    /// </summary>
    private static string HashPassword(string password)
    {
        // تولید Salt تصادفی
        var salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // هش کردن با PBKDF2
        var key = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: KeySize
        );

        // ترکیب نتایج
        var hash = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hash, 0, SaltSize);
        Array.Copy(key, 0, hash, SaltSize, KeySize);

        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// تایید رمز عبور
    /// </summary>
    private static bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);

            // استخراج Salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // محاسبه مجدد هش
            var key = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize
            );

            // مقایسه امن
            return CryptographicEquals(
                hashBytes.Skip(SaltSize).ToArray(),
                key
            );
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// مقایسه امن دو آرایه بایت (جلوگیری از timing attacks)
    /// </summary>
    private static bool CryptographicEquals(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        var result = 0;
        for (var i = 0; i < a.Length; i++)
            result |= a[i] ^ b[i];

        return result == 0;
    }

    /// <summary>
    /// اعتبارسنجی پیچیدگی رمز عبور
    /// </summary>
    private static bool IsValidPasswordComplexity(string password)
    {
        // بررسی پیچیدگی
        var hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
        var hasLowerCase = Regex.IsMatch(password, @"[a-z]");
        var hasDigit = Regex.IsMatch(password, @"\d");
        var hasSpecialChar = Regex.IsMatch(password, @"[\W_]");

        // حداقل 3 از 4 شرط باید برقرار باشد
        var complexityScore = 0;
        if (hasUpperCase) complexityScore++;
        if (hasLowerCase) complexityScore++;
        if (hasDigit) complexityScore++;
        if (hasSpecialChar) complexityScore++;

        return complexityScore >= 3;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return HashedValue;
    }

    public override string ToString() => "***PROTECTED***";
}