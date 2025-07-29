using AuthSystem.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace AuthSystem.Infrastructure.Services.PasswordHasher;

/// <summary>
/// پیاده‌سازی IPasswordHasher با استفاده از PBKDF2
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// هش کردن یک رمز عبور
    /// </summary>
    /// <param name="password">رمز عبور متن ساده</param>
    /// <returns>هش رمز عبور</returns>
    public string HashPassword(string password)
    {
        // ایجاد salt تصادفی
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // ایجاد هش با استفاده از PBKDF2
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // بازگرداندن salt و هش به صورت ترکیبی
        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    /// <summary>
    /// تأیید یک رمز عبور با هش آن
    /// </summary>
    /// <param name="hashedPassword">هش رمز عبور</param>
    /// <param name="providedPassword">رمز عبور ارائه شده</param>
    /// <returns>در صورت مطابقت true باز می‌گرداند</returns>
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
            return false;

        var parts = hashedPassword.Split('.');
        if (parts.Length != 2)
            return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hash = parts[1];

        var test = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: providedPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hash == test;
    }
}