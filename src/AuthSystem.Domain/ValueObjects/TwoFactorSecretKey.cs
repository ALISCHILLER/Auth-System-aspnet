using AuthSystem.Domain.Common.Base;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value object for two-factor authentication secret keys.
/// </summary>
public sealed class TwoFactorSecretKey : ValueObject
{

    private const int SecretKeyLength = 20;

    private const string DefaultIssuer = "AuthSystem";

    private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
    public string Value { get; private set; }
    public string Issuer { get; private set; }


    public DateTime CreatedAt { get; private set; }

    public bool IsActive { get; private set; }


    public DateTime? LastUsedAt { get; private set; }

    private TwoFactorSecretKey()
    {
        Value = string.Empty;
        Issuer = DefaultIssuer;
        CreatedAt = DomainClock.Instance.UtcNow;
    }


    private TwoFactorSecretKey(string value, string issuer, bool isActive = false, DateTime? createdAt = null)
    {
        Value = value;
        Issuer = issuer;
        CreatedAt = createdAt.HasValue ? EnsureUtc(createdAt.Value) : DomainClock.Instance.UtcNow;
        IsActive = isActive;
    }


    public static TwoFactorSecretKey Generate(string issuer = DefaultIssuer)
    {
        if (string.IsNullOrWhiteSpace(issuer))
        {
            issuer = DefaultIssuer;
        }

        var secretKey = GenerateSecretKey();
        return new TwoFactorSecretKey(secretKey, issuer);
    }

    public static TwoFactorSecretKey CreateFromExisting(string value, string issuer, bool isActive, DateTime createdAt, DateTime? lastUsedAt = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("کلید محرمانه نمی‌تواند خالی باشد", nameof(value));
        }

        if (!IsValidBase32(value))
        {
            throw new InvalidTwoFactorSecretKeyException("فرمت کلید محرمانه نامعتبر است");
        }

        if (string.IsNullOrWhiteSpace(issuer))
        {
            issuer = DefaultIssuer;
        }

        var key = new TwoFactorSecretKey(value, issuer, isActive, createdAt)
        {
            LastUsedAt = lastUsedAt.HasValue ? EnsureUtc(lastUsedAt.Value) : null
        };

        return key;
    }


    private static string GenerateSecretKey()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[SecretKeyLength];
        rng.GetBytes(bytes);
        return Base32Encode(bytes);
    }


    private static string Base32Encode(byte[] data)
    {

        var result = new StringBuilder((data.Length + 4) / 5 * 8);

        for (var i = 0; i < data.Length; i += 5)
        {
            var chunk = new byte[5];
            var chunkLength = Math.Min(5, data.Length - i);
            Array.Copy(data, i, chunk, 0, chunkLength);

            result.Append(Base32Alphabet[(chunk[0] >> 3) & 0x1F]);
            result.Append(Base32Alphabet[((chunk[0] << 2) | (chunk[1] >> 6)) & 0x1F]);

            if (chunkLength > 1)
            {
                result.Append(Base32Alphabet[(chunk[1] >> 1) & 0x1F]);
                result.Append(Base32Alphabet[((chunk[1] << 4) | (chunk[2] >> 4)) & 0x1F]);
            }

            if (chunkLength > 2)
            {
                result.Append(Base32Alphabet[((chunk[2] << 1) | (chunk[3] >> 7)) & 0x1F]);
            }

            if (chunkLength > 3)
            {
                result.Append(Base32Alphabet[(chunk[3] >> 2) & 0x1F]);
                result.Append(Base32Alphabet[((chunk[3] << 3) | (chunk[4] >> 5)) & 0x1F]);
            }

            if (chunkLength > 4)
            {
                result.Append(Base32Alphabet[chunk[4] & 0x1F]);
            }
        }

        return result.ToString();
    }


    private static bool IsValidBase32(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        foreach (var c in value)
        {
            if (!Base32Alphabet.Contains(c))
            {
                return false;
            }
        }

        return true;
    }


    public string GenerateUri(string userIdentifier)
    {
        if (string.IsNullOrWhiteSpace(userIdentifier))
        {
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userIdentifier));
        }

        return $"otpauth://totp/{Uri.EscapeDataString(Issuer)}:{Uri.EscapeDataString(userIdentifier)}" +
               $"?secret={Value}&issuer={Uri.EscapeDataString(Issuer)}";
    }


    public TwoFactorSecretKey Activate()
    {
        var activated = new TwoFactorSecretKey(Value, Issuer, true, CreatedAt)
        {
            LastUsedAt = DomainClock.Instance.UtcNow
        };

        return activated;
    }


    public TwoFactorSecretKey Deactivate()
    {
        var deactivated = new TwoFactorSecretKey(Value, Issuer, false, CreatedAt)
        {
            LastUsedAt = LastUsedAt
        };

        return deactivated;
    }


    public TwoFactorSecretKey RecordUsage()
    {
        var updated = new TwoFactorSecretKey(Value, Issuer, IsActive, CreatedAt)
        {
            LastUsedAt = DomainClock.Instance.UtcNow
        };

        return updated;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Issuer;
    }

    public override string ToString() => "[2FA Secret Key]";

    private static DateTime EnsureUtc(DateTime dateTime) => dateTime.Kind switch
    {
        DateTimeKind.Utc => dateTime,
        DateTimeKind.Local => dateTime.ToUniversalTime(),
        _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
    };
}