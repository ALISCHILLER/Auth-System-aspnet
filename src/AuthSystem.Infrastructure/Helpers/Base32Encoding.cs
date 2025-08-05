using System;
using System.Text;

namespace AuthSystem.Infrastructure.Helpers;

/// <summary>
/// کلاس کمکی برای کدگذاری و رمزگشایی Base32
/// برای سازگاری با Google Authenticator و احراز هویت دو مرحله‌ای
/// </summary>
public static class Base32Encoding
{
    private const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    /// <summary>
    /// تبدیل آرایه بایت به رشته Base32
    /// </summary>
    public static string ToString(byte[] input)
    {
        if (input == null || input.Length == 0)
            return string.Empty;

        var sb = new StringBuilder();
        var bits = 0;
        var value = 0;

        for (var i = 0; i < input.Length; i++)
        {
            value = (value << 8) | input[i];
            bits += 8;

            while (bits >= 5)
            {
                sb.Append(Digits[(value >> (bits - 5)) & 31]);
                bits -= 5;
            }
        }

        if (bits > 0)
        {
            sb.Append(Digits[(value << (5 - bits)) & 31]);
        }

        // اضافه کردن padding
        while (sb.Length % 8 != 0)
        {
            sb.Append('=');
        }

        return sb.ToString();
    }

    /// <summary>
    /// تبدیل رشته Base32 به آرایه بایت
    /// </summary>
    public static byte[] ToBytes(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Array.Empty<byte>();

        // حذف padding و فاصله‌ها
        input = input.TrimEnd('=').Replace(" ", "").Replace("-", "").ToUpper();

        var output = new byte[input.Length * 5 / 8];
        var bits = 0;
        var value = 0;
        var outputIndex = 0;

        foreach (var c in input)
        {
            var digit = Digits.IndexOf(c);
            if (digit == -1)
                throw new ArgumentException($"کاراکتر نامعتبر Base32: {c}");

            value = (value << 5) | digit;
            bits += 5;

            if (bits >= 8)
            {
                output[outputIndex++] = (byte)(value >> (bits - 8));
                bits -= 8;
            }
        }

        return output;
    }
}