using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Common.Clock;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.Verification;

internal sealed class InMemoryVerificationCodeService : IVerificationCodeService
{
    private readonly ILogger<InMemoryVerificationCodeService> _logger;
    private readonly ConcurrentDictionary<(Guid UserId, CodeKind Kind), VerificationCodeEntry> _codes = new();
    private static readonly TimeSpan DefaultLifetime = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan TwoFactorLifetime = TimeSpan.FromMinutes(2);

    public InMemoryVerificationCodeService(ILogger<InMemoryVerificationCodeService> logger)
    {
        _logger = logger;
    }

    public Task<string> GenerateEmailVerificationCodeAsync(Guid userId, CancellationToken ct)
    {
        var code = GenerateCode();
        var entry = new VerificationCodeEntry(code, DomainClock.Instance.UtcNow.Add(DefaultLifetime));
        _codes[(userId, CodeKind.Email)] = entry;
        _logger.LogDebug("Generated email verification code for user {UserId}", userId);
        return Task.FromResult(code);
    }

    public Task<bool> ValidateEmailVerificationCodeAsync(Guid userId, string code, CancellationToken ct)
        => Task.FromResult(ValidateCode(userId, CodeKind.Email, code));

    public Task<string> GenerateTwoFactorCodeAsync(Guid userId, CancellationToken ct)
    {
        var code = GenerateCode();
        var entry = new VerificationCodeEntry(code, DomainClock.Instance.UtcNow.Add(TwoFactorLifetime));
        _codes[(userId, CodeKind.TwoFactor)] = entry;
        _logger.LogDebug("Generated 2FA code for user {UserId}", userId);
        return Task.FromResult(code);
    }

    public Task<bool> ValidateTwoFactorCodeAsync(Guid userId, string code, CancellationToken ct)
        => Task.FromResult(ValidateCode(userId, CodeKind.TwoFactor, code));

    private bool ValidateCode(Guid userId, CodeKind kind, string code)
    {
        if (!_codes.TryGetValue((userId, kind), out var entry))
        {
            return false;
        }

        if (entry.IsExpired())
        {
            _codes.TryRemove((userId, kind), out _);
            return false;
        }

        var isValid = string.Equals(entry.Code, code, StringComparison.Ordinal);
        if (isValid)
        {
            _codes.TryRemove((userId, kind), out _);
        }

        return isValid;
    }

    private static string GenerateCode() => Random.Shared.Next(100000, 999999).ToString();

    private enum CodeKind
    {
        Email,
        TwoFactor
    }

    private sealed record VerificationCodeEntry(string Code, DateTime ExpiresAt)
    {
        public bool IsExpired() => DomainClock.Instance.UtcNow >= ExpiresAt;
    }
}