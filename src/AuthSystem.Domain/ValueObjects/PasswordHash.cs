using System;
using System.Collections.Generic;
using System.Security.Cryptography; 
using System.Text;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Base;
using AuthSystem.Domain.Enums; 
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value object encapsulating hashed passwords.
/// </summary>
public sealed class PasswordHash : ValueObject
{
    
    private const int ExpectedHashLength = 60;

    
    public string Value { get; }

    public HashAlgorithm Algorithm { get; }
    public DateTime CreatedAt { get; }

    public bool NeedsRehash => CreatedAt < DomainClock.Instance.UtcNow.AddMonths(-6);

    private PasswordHash(string value, HashAlgorithm algorithm, DateTime? createdAt = null)
    {
        Value = value;
        Algorithm = algorithm;
        CreatedAt = createdAt?.ToUniversalTime() ?? DomainClock.Instance.UtcNow;
    }

   
    public static PasswordHash CreateFromPlainText(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
        {
            throw new InvalidPasswordException("رمز عبور نمی‌تواند خالی باشد");
        }

        var hash = HashPassword(plainPassword);
        return new PasswordHash(hash, HashAlgorithm.BCrypt);
    }

   
    public static PasswordHash CreateFromHash(string hash, DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            throw new ArgumentNullException(nameof(hash));
        }

        var algorithm = DetectAlgorithm(hash);
        return new PasswordHash(hash, algorithm, createdAt);
    }

   
    public bool Verify(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
        {
            return false;
        }

        return VerifyPassword(plainPassword, Value);
    }

  
    public PasswordHash RehashIfNeeded(string plainPassword)
    {
        return NeedsRehash ? CreateFromPlainText(plainPassword) : this;
    }

    
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt"));
        var hash = Convert.ToBase64String(bytes);
        return hash.Length >= ExpectedHashLength ? hash[..ExpectedHashLength] : hash;
    }

    private static bool VerifyPassword(string password, string hash) => HashPassword(password) == hash;

   
    private static HashAlgorithm DetectAlgorithm(string hash)
    {
        if (hash.StartsWith("$2a$") || hash.StartsWith("$2b$") || hash.StartsWith("$2y$"))
        {
            return HashAlgorithm.BCrypt;
        }

        if (hash.StartsWith("$argon2"))
        {
            return HashAlgorithm.Argon2;
        }

        return HashAlgorithm.PBKDF2;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Algorithm;
    }

    public override string ToString() => $"[PROTECTED HASH - {Algorithm}]";
}