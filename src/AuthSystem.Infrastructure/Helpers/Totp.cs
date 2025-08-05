using System;
using System.Security.Cryptography;

namespace AuthSystem.Infrastructure.Helpers;

/// <summary>
/// پیاده‌سازی ساده TOTP (Time-based One-Time Password)
/// برای احراز هویت دو مرحله‌ای
/// </summary>
public class Totp
{
    private readonly byte[] _secretKey;
    private readonly int _step;
    private readonly int _codeSize;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="secretKey">کلید محرمانه</param>
    /// <param name="step">گام زمانی (به ثانیه)</param>
    /// <param name="totpSize">طول کد</param>
    public Totp(byte[] secretKey, int step = 30, int totpSize = 6)
    {
        _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
        _step = step;
        _codeSize = totpSize;
    }

    /// <summary>
    /// محاسبه کد TOTP برای زمان فعلی
    /// </summary>
    public string ComputeTotp()
    {
        var counter = GetCounter();
        return ComputeTotp(counter);
    }

    /// <summary>
    /// محاسبه کد TOTP برای یک کانتر مشخص
    /// </summary>
    private string ComputeTotp(long counter)
    {
        var counterBytes = BitConverter.GetBytes(counter);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(counterBytes);

        using var hmac = new HMACSHA1(_secretKey);
        var hash = hmac.ComputeHash(counterBytes);

        var offset = hash[hash.Length - 1] & 0x0F;
        var binary = ((hash[offset] & 0x7F) << 24) |
                     ((hash[offset + 1] & 0xFF) << 16) |
                     ((hash[offset + 2] & 0xFF) << 8) |
                     (hash[offset + 3] & 0xFF);

        var password = (binary % 1000000).ToString();
        while (password.Length < _codeSize)
            password = "0" + password;

        return password.Substring(password.Length - _codeSize);
    }

    /// <summary>
    /// دریافت کانتر بر اساس زمان فعلی
    /// </summary>
    private long GetCounter()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds / _step;
    }
}