using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Enums.AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using System.Security.Cryptography;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object برای کد تایید با انقضا و محدودیت تلاش
/// </summary>
public sealed class VerificationCode : ValueObject
{
    private const int DefaultLength = 6;
    private const int MinLength = 4;
    private const int MaxLength = 10;
    private const int MaxAttempts = 3;

    public string Value { get; }
    public VerificationCodeType Type { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }
    public int AttemptCount { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime? UsedAt { get; private set; }

    private VerificationCode(string value, VerificationCodeType type, DateTime expiresAt, int attemptCount = 0, bool isUsed = false, DateTime? usedAt = null)
    {
        Value = value;
        Type = type;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        AttemptCount = attemptCount;
        IsUsed = isUsed;
        UsedAt = usedAt;
    }

    public static VerificationCode GenerateNumeric(VerificationCodeType type, int length = DefaultLength, int? validityMinutes = null)
    {
        ValidateLength(length);
        var code = GenerateNumericCode(length);
        var validity = validityMinutes ?? 10;
        var expiresAt = DateTime.UtcNow.AddMinutes(validity);
        return new VerificationCode(code, type, expiresAt);
    }

    private static void ValidateLength(int length)
    {
        if (length < MinLength || length > MaxLength)
            throw new ArgumentException($"طول کد باید بین {MinLength} و {MaxLength} کاراکتر باشد");
    }

    private static string GenerateNumericCode(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var randomNumber = BitConverter.ToUInt32(bytes, 0);
        var code = (randomNumber % (uint)Math.Pow(10, length)).ToString();
        return code.PadLeft(length, '0');
    }

    public bool Verify(string code)
    {
        if (!IsValid()) return false;
        AttemptCount++;
        if (string.IsNullOrWhiteSpace(code)) return false;
        var isMatch = CryptographicEquals(Value, code);
        if (isMatch)
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }
        return isMatch;
    }

    private static bool CryptographicEquals(string a, string b)
    {
        if (a.Length != b.Length) return false;
        var result = 0;
        for (var i = 0; i < a.Length; i++)
            result |= a[i] ^ b[i];
        return result == 0;
    }

    public bool IsValid() => !IsExpired && !IsUsed && AttemptCount < MaxAttempts;
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
        yield return CreatedAt;
    }
    public override string ToString() => $"[{Type} Code: ***]";
}