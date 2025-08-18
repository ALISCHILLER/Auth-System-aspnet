using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object برای کلید مخفی احراز هویت دو عاملی
/// </summary>
public sealed class TwoFactorSecretKey : ValueObject
{
    private const int SecretKeyLength = 32; // 256 bits

    public string Value { get; }
    public byte[] KeyBytes { get; }

    private TwoFactorSecretKey(string value, byte[] keyBytes)
    {
        Value = value;
        KeyBytes = keyBytes;
    }

    /// <summary>
    /// تولید کلید جدید
    /// </summary>
    public static TwoFactorSecretKey Generate()
    {
        var keyBytes = new byte[SecretKeyLength];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }

        var base32Key = Base32Encode(keyBytes);
        return new TwoFactorSecretKey(base32Key, keyBytes);
    }

    /// <summary>
    /// بازیابی از مقدار ذخیره شده
    /// </summary>
    public static TwoFactorSecretKey Create(string base32Value)
    {
        if (string.IsNullOrWhiteSpace(base32Value))
            throw new ArgumentNullException(nameof(base32Value));

        try
        {
            var keyBytes = Base32Decode(base32Value);
            if (keyBytes.Length != SecretKeyLength)
                throw InvalidTwoFactorSecretKeyException.ForInvalidLength(keyBytes.Length, SecretKeyLength);

            return new TwoFactorSecretKey(base32Value, keyBytes);
        }
        catch (Exception ex) when (ex is not DomainException)
        {
            throw InvalidTwoFactorSecretKeyException.ForInvalidFormat(base32Value, ex);
        }
    }

    /// <summary>
    /// تولید URI برای QR Code
    /// </summary>
    public string GenerateUri(string issuer, string accountIdentifier)
    {
        return $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(accountIdentifier)}?" +
               $"secret={Value}&issuer={Uri.EscapeDataString(issuer)}";
    }

    /// <summary>
    /// Base32 Encoding (RFC 4648)
    /// </summary>
    private static string Base32Encode(byte[] data)
    {
        const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var result = new StringBuilder();

        for (int i = 0; i < data.Length; i += 5)
        {
            var chunk = new byte[8];
            int chunkLength = Math.Min(5, data.Length - i);

            for (int j = 0; j < chunkLength; j++)
                chunk[7 - j] = data[i + j];

            ulong value = BitConverter.ToUInt64(chunk, 0);

            for (int j = 0; j < 8; j++)
            {
                if (j * 5 / 8 < chunkLength)
                {
                    int index = (int)((value >> (7 - j) * 5) & 0x1F);
                    result.Append(base32Chars[index]);
                }
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Base32 Decoding
    /// </summary>
    private static byte[] Base32Decode(string encoded)
    {
        if (string.IsNullOrWhiteSpace(encoded))
            throw new ArgumentException("Encoded string cannot be empty");

        encoded = encoded.TrimEnd('=').ToUpperInvariant();
        const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        var bits = new List<byte>();
        foreach (char c in encoded)
        {
            int charIndex = base32Chars.IndexOf(c);
            if (charIndex < 0)
                throw new FormatException($"Invalid character in base32 string: {c}");

            for (int i = 4; i >= 0; i--)
            {
                bits.Add((byte)((charIndex >> i) & 1));
            }
        }

        var bytes = new List<byte>();
        for (int i = 0; i + 7 < bits.Count; i += 8)
        {
            byte b = 0;
            for (int j = 0; j < 8; j++)
            {
                b = (byte)((b << 1) | bits[i + j]);
            }
            bytes.Add(b);
        }

        return bytes.ToArray();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => "***SECRET***";
}
