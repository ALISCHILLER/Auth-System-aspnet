using AuthSystem.Domain.Exceptions.Infrastructure;
using AuthSystem.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace AuthSystem.Infrastructure.Services.PasswordHasher;

/// <summary>
/// پیاده‌سازی IPasswordHasher با استفاده از ASP.NET Core Data Protection
/// این کلاس برای هش کردن و تأیید رمز عبور استفاده می‌شود
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// هش کردن یک رمز عبور با استفاده از PBKDF2
    /// </summary>
    /// <param name="password">رمز عبور متن ساده</param>
    /// <returns>هش رمز عبور به همراه سالت</returns>
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("رمز عبور نمی‌تواند خالی باشد", nameof(password));

        try
        {
            // تولید سالت تصادفی
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // هش کردن رمز عبور با استفاده از PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // ترکیب سالت و هش
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در هش کردن رمز عبور.", ex);
        }
    }

    /// <summary>
    /// تأیید یک رمز عبور با هش آن
    /// </summary>
    /// <param name="hashedPassword">هش رمز عبور (سالت.هش)</param>
    /// <param name="providedPassword">رمز عبور ارائه شده</param>
    /// <returns>در صورت مطابقت true باز می‌گرداند</returns>
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
            return false;

        try
        {
            // جداسازی سالت و هش
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            // هش کردن رمز عبور ارائه شده با همان سالت
            var hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // مقایسه هش‌ها
            return hash.Equals(hashedInput, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception)
        {
            return false;
        }
    }
}