using AuthSystem.Domain.Common.Base;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value object representing secure tokens.
/// </summary>
public sealed class TokenValue : ValueObject
{

    private const int MinLength = 32;

    private const int MaxLength = 512;


    private static readonly Regex TokenPattern = new(
        @"^[A-Za-z0-9_-]+$",
        RegexOptions.Compiled);


    public string Value { get; }


    public TokenType Type { get; }


    public DateTime CreatedAt { get; }


    public DateTime? ExpiresAt { get; }

    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DomainClock.Instance.UtcNow;
    public TimeSpan? TimeToExpiry => ExpiresAt.HasValue && !IsExpired
       ? ExpiresAt.Value - DomainClock.Instance.UtcNow
        : null;

    private TokenValue(string value, TokenType type, DateTime createdAt, DateTime? expiresAt)
    {
        Value = value;
        Type = type;
        CreatedAt = EnsureUtc(createdAt);
        ExpiresAt = expiresAt.HasValue ? EnsureUtc(expiresAt.Value) : null;
    }


    public static TokenValue Create(string value, TokenType type, DateTime? expiresAt = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidTokenException("توکن نمی‌تواند خالی باشد");
        }

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            throw new InvalidTokenException($"طول توکن باید بین {MinLength} و {MaxLength} کاراکتر باشد");
        }

        if (!TokenPattern.IsMatch(value))
        {
            throw new InvalidTokenException("فرمت توکن نامعتبر است");
        }

        if (expiresAt.HasValue && expiresAt.Value <= DomainClock.Instance.UtcNow)
        {
            throw new InvalidTokenException("زمان انقضای توکن باید در آینده باشد");
        }

        var createdAt = DomainClock.Instance.UtcNow;
        return new TokenValue(value, type, createdAt, expiresAt);
    }


    public static TokenValue Generate(TokenType type, int length = 64, TimeSpan? validity = null)
    {
        if (length < MinLength || length > MaxLength)
        {
            throw new ArgumentException($"طول توکن باید بین {MinLength} و {MaxLength} کاراکتر باشد", nameof(length));
        }

        var token = GenerateSecureToken(length);
        var now = DomainClock.Instance.UtcNow;
        var expiresAt = validity.HasValue ? now.Add(validity.Value) : (DateTime?)null;
        return new TokenValue(token, type, now, expiresAt);
    }


    private static string GenerateSecureToken(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length * 3 / 4];
        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=')
            .Substring(0, length);
    }


    public string GetHash()
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Value));
        return Convert.ToBase64String(bytes);
    }

    public bool IsValid() => !IsExpired;


    public TokenValue Expire()
    {
        var now = DomainClock.Instance.UtcNow;
        return new TokenValue(Value, Type, CreatedAt, now.AddSeconds(-1));
    }


    public TokenValue Extend(TimeSpan extension)
    {
        var baseTime = ExpiresAt ?? DomainClock.Instance.UtcNow;
        return new TokenValue(Value, Type, CreatedAt, baseTime.Add(extension));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
    }

    public override string ToString() => $"[{Type} Token: ***]";
    private static DateTime EnsureUtc(DateTime dateTime) => dateTime.Kind switch
    {
        DateTimeKind.Utc => dateTime,
        DateTimeKind.Local => dateTime.ToUniversalTime(),
        _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
    };
}