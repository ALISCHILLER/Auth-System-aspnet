using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
    /// هش کردن داده
    /// </summary>
    public string Hash(string input, string salt = null)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + salt));
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// اعتبارسنجی هش
    /// </summary>
    public bool VerifyHash(string input, string hash, string salt = null)
    {
        var computedHash = Hash(input, salt);
        return computedHash == hash;
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