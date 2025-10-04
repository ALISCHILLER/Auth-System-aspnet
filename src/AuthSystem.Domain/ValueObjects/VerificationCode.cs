using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value object representing verification codes with expiry semantics.
/// </summary>
public sealed class VerificationCode : ValueObject
{
    
    private const int DefaultLength = 6;

   
    private const int MinLength = 4;

   
    private const int MaxLength = 10;

   
    private const int DefaultValidityMinutes = 10;

   
    private const int MaxAttempts = 3;

   
    public string Value { get; }

    
    public VerificationCodeType Type { get; }

   
    public DateTime CreatedAt { get; }

  
    public DateTime ExpiresAt { get; }


    public int AttemptCount { get; private set; }

    
    public bool IsUsed { get; private set; }

  
    public DateTime? UsedAt { get; private set; }

    public bool IsExpired => DomainClock.Instance.UtcNow > ExpiresAt;
    public bool IsValid => !IsExpired && !IsUsed && AttemptCount < MaxAttempts;

 
    public int RemainingAttempts => Math.Max(0, MaxAttempts - AttemptCount);

    public TimeSpan? TimeToExpiry => IsExpired ? null : ExpiresAt - DomainClock.Instance.UtcNow;
    private VerificationCode(
        string value,
        VerificationCodeType type,
         DateTime createdAt,
        DateTime expiresAt,
        int attemptCount = 0,
        bool isUsed = false,
        DateTime? usedAt = null)
    {
        Value = value;
        Type = type;
        CreatedAt = EnsureUtc(createdAt);
        ExpiresAt = EnsureUtc(expiresAt);
        AttemptCount = attemptCount;
        IsUsed = isUsed;
        UsedAt = usedAt.HasValue ? EnsureUtc(usedAt.Value) : null;
    }

    public static VerificationCode GenerateNumeric(VerificationCodeType type, int length = DefaultLength, int? validityMinutes = null)
    {
        ValidateLength(length);
        var now = DomainClock.Instance.UtcNow;
        var expiresAt = now.AddMinutes(validityMinutes ?? DefaultValidityMinutes);
        var code = GenerateNumericCode(length);
        return new VerificationCode(code, type, now, expiresAt);
    }

    public static VerificationCode GenerateAlphanumeric(VerificationCodeType type, int length = DefaultLength, int? validityMinutes = null)
    {
        ValidateLength(length);
        var now = DomainClock.Instance.UtcNow;
        var expiresAt = now.AddMinutes(validityMinutes ?? DefaultValidityMinutes);
        var code = GenerateAlphanumericCode(length);
        return new VerificationCode(code, type, now, expiresAt);
    }

  
    public static VerificationCode CreateFromExisting(
        string value,
        VerificationCodeType type,
        DateTime createdAt,
        DateTime expiresAt,
        int attemptCount = 0,
        bool isUsed = false,
        DateTime? usedAt = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("کد تایید نمی‌تواند خالی باشد", nameof(value));
        }

        return new VerificationCode(value, type, createdAt, expiresAt, attemptCount, isUsed, usedAt);
    }

   
    public bool Verify(string code)
    {
        if (!IsValid)
        {
            return false;
        }

       
        AttemptCount++;

        if (string.IsNullOrWhiteSpace(code))
        {
            return false;
        }

      
        var isMatch = Value.Equals(code, StringComparison.OrdinalIgnoreCase);

        if (isMatch)
        {
            IsUsed = true;
            UsedAt = DomainClock.Instance.UtcNow;
        }

        return isMatch;
    }

    public VerificationCode RecordFailedAttempt() =>
       new(Value, Type, CreatedAt, ExpiresAt, AttemptCount + 1, IsUsed, UsedAt);

    public VerificationCode MarkAsUsed() =>
       new(Value, Type, CreatedAt, ExpiresAt, AttemptCount, true, DomainClock.Instance.UtcNow);

    
    private static void ValidateLength(int length)
    {
        if (length < MinLength || length > MaxLength)
        {
            throw new ArgumentException($"طول کد باید بین {MinLength} و {MaxLength} کاراکتر باشد", nameof(length));
        }
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

    
    private static string GenerateAlphanumericCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        using var rng = RandomNumberGenerator.Create();
        var result = new char[length];
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        for (var i = 0; i < length; i++)
        {
            result[i] = chars[bytes[i] % chars.Length];
        }
        return new string(result);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
        yield return CreatedAt;
    }

    private static DateTime EnsureUtc(DateTime dateTime) => dateTime.Kind switch
    {
        DateTimeKind.Utc => dateTime,
        DateTimeKind.Local => dateTime.ToUniversalTime(),
        _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
    };
}