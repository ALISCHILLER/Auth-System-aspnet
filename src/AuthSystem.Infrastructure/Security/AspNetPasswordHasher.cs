using System;
using AuthSystem.Application.Common.Abstractions.Security;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Infrastructure.Security;

internal sealed class AspNetPasswordHasher : IPasswordHasher
{
    private static readonly object SharedUser = new();
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        }

        return _passwordHasher.HashPassword(SharedUser, password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
        {
            return false;
        }

        var result = _passwordHasher.VerifyHashedPassword(SharedUser, hashedPassword, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}