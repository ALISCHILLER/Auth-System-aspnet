using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DomainHashAlgorithm = AuthSystem.Domain.Enums.HashAlgorithm; // استفاده از alias
using AuthSystem.Domain.Services.Contracts;

namespace AuthSystem.Domain.Common.Mocks;

/// <summary>
/// شبیه‌سازی ارائه‌دهنده رمزنگاری برای تست‌ها
/// </summary>
public class MockCryptoProvider : ICryptoProvider
{
    private readonly Dictionary<string, string> _encryptedValues = new();
    private readonly Dictionary<string, string> _decryptedValues = new();

    /// <summary>
    /// رمزنگاری داده
    /// </summary>
    public string Encrypt(string plainText, string key)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException(nameof(plainText));

        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        var encrypted = $"{plainText}|{key}|ENCRYPTED";
        _encryptedValues[plainText] = encrypted;
        _decryptedValues[encrypted] = plainText;

        return encrypted;
    }

    /// <summary>
    /// رمزگشایی داده
    /// </summary>
    public string Decrypt(string cipherText, string key)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentNullException(nameof(cipherText));

        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        if (_decryptedValues.TryGetValue(cipherText, out var decrypted))
            return decrypted;

        // در حالت عادی باید رمزگشایی انجام شود
        // این فقط یک شبیه‌سازی است
        return cipherText.Replace($"|{key}|ENCRYPTED", "");
    }

    /// <summary>
    /// ایجاد کلید تصادفی
    /// </summary>
    public string GenerateRandomKey(int length = 32)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// ایجاد IV تصادفی
    /// </summary>
    public string GenerateRandomIv(int length = 16)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// هش کردن داده با الگوریتم مشخص
    /// </summary>
    public string Hash(string input, string salt = null, DomainHashAlgorithm algorithm = DomainHashAlgorithm.SHA256)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));

        // بر اساس الگوریتم انتخابی عمل می‌کنیم
        byte[] bytes;
        switch (algorithm)
        {
            case DomainHashAlgorithm.SHA256:
                using (var sha256 = SHA256.Create())
                {
                    bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + (salt ?? string.Empty)));
                }
                break;

            case DomainHashAlgorithm.SHA512:
                using (var sha512 = SHA512.Create())
                {
                    bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(input + (salt ?? string.Empty)));
                }
                break;

            case DomainHashAlgorithm.MD5:
                using (var md5 = MD5.Create())
                {
                    bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input + (salt ?? string.Empty)));
                }
                break;

            default:
                // برای الگوریتم‌های دیگر، SHA256 را به عنوان پیش‌فرض استفاده می‌کنیم
                using (var sha256 = SHA256.Create())
                {
                    bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + (salt ?? string.Empty)));
                }
                break;
        }

        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// تأیید هش داده با استفاده از هش موجود و نمک
    /// </summary>
    public bool VerifyHash(string input, string hash, string salt = null, DomainHashAlgorithm algorithm = DomainHashAlgorithm.SHA256)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));

        if (string.IsNullOrEmpty(hash))
            throw new ArgumentNullException(nameof(hash));

        var computedHash = Hash(input, salt, algorithm);
        return computedHash == hash;
    }

    /// <summary>
    /// ایجاد توکن امن تصادفی
    /// </summary>
    public string GenerateSecureToken(int length = 64)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes)
            .Replace("/", "_")
            .Replace("+", "-")
            .Substring(0, length);
    }

    /// <summary>
    /// رمزنگاری داده با استفاده از کلید عمومی
    /// </summary>
    public string EncryptWithPublicKey(string plainText, string publicKey)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException(nameof(plainText));

        if (string.IsNullOrEmpty(publicKey))
            throw new ArgumentNullException(nameof(publicKey));

        // در حالت واقعی باید از RSA یا الگوریتم‌های مشابه استفاده کنیم
        // این فقط یک شبیه‌سازی ساده است
        return $"{plainText}|PUBLIC_KEY_ENCRYPTED";
    }

    /// <summary>
    /// رمزگشایی داده با استفاده از کلید خصوصی
    /// </summary>
    public string DecryptWithPrivateKey(string cipherText, string privateKey)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentNullException(nameof(cipherText));

        if (string.IsNullOrEmpty(privateKey))
            throw new ArgumentNullException(nameof(privateKey));

        // در حالت واقعی باید از RSA یا الگوریتم‌های مشابه استفاده کنیم
        // این فقط یک شبیه‌سازی ساده است
        return cipherText.Replace("|PUBLIC_KEY_ENCRYPTED", "");
    }

    /// <summary>
    /// ایجاد امضای دیجیتال برای داده
    /// </summary>
    public string SignData(string data, string privateKey)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentNullException(nameof(data));

        if (string.IsNullOrEmpty(privateKey))
            throw new ArgumentNullException(nameof(privateKey));

        // در حالت واقعی باید از RSA یا الگوریتم‌های امضای دیجیتال استفاده کنیم
        // این فقط یک شبیه‌سازی ساده است
        return $"{data}|SIGNATURE";
    }

    /// <summary>
    /// تأیید امضای دیجیتال برای داده
    /// </summary>
    public bool VerifySignature(string data, string signature, string publicKey)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentNullException(nameof(data));

        if (string.IsNullOrEmpty(signature))
            throw new ArgumentNullException(nameof(signature));

        if (string.IsNullOrEmpty(publicKey))
            throw new ArgumentNullException(nameof(publicKey));

        // در حالت واقعی باید از RSA یا الگوریتم‌های امضای دیجیتال استفاده کنیم
        // این فقط یک شبیه‌سازی ساده است
        return signature.StartsWith(data + "|SIGNATURE");
    }

    /// <summary>
    /// ریست کردن وضعیت شبیه‌سازی
    /// </summary>
    public void Reset()
    {
        _encryptedValues.Clear();
        _decryptedValues.Clear();
    }
}