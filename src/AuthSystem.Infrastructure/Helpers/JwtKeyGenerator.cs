using System.Security.Cryptography;
using System.Text;

namespace AuthSystem.Infrastructure.Helpers;

/// <summary>
/// کلاس کمکی برای ایجاد کلید مخفی قوی برای JWT
/// </summary>
public static class JwtKeyGenerator
{
    /// <summary>
    /// ایجاد یک کلید مخفی تصادفی و قوی برای JWT
    /// </summary>
    /// <param name="length">طول کلید (به بایت)</param>
    /// <returns>کلید مخفی به صورت Base64</returns>
    public static string GenerateStrongSecretKey(int length = 32)
    {
        if (length < 32)
            throw new ArgumentException("طول کلید باید حداقل 32 بایت باشد.", nameof(length));

        var bytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// ایجاد یک کلید مخفی با استفاده از روش PBKDF2
    /// </summary>
    /// <param name="password">رمز عبور</param>
    /// <param name="salt">سالت</param>
    /// <param name="length">طول کلید (به بایت)</param>
    /// <returns>کلید مخفی به صورت Base64</returns>
    public static string GenerateSecretKeyFromPassword(string password, string salt, int length = 32)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("رمز عبور نمی‌تواند خالی باشد.", nameof(password));

        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentException("سالت نمی‌تواند خالی باشد.", nameof(salt));

        using var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000, HashAlgorithmName.SHA256);
        var key = pbkdf2.GetBytes(length);
        return Convert.ToBase64String(key);
    }
}