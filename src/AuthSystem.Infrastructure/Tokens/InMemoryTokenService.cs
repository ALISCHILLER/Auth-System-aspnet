using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Entities.UserAggregate;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthSystem.Infrastructure.Tokens;

internal sealed class InMemoryTokenService : ITokenService
{
    private readonly ILogger<InMemoryTokenService> _logger;
    private readonly TokenOptions _options;
    private readonly ConcurrentDictionary<string, TokenDescriptor> _accessTokens = new();
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, TokenDescriptor>> _refreshTokens = new();

    public InMemoryTokenService(
        ILogger<InMemoryTokenService> logger,
        IOptions<TokenOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public Task<string> CreateAccessTokenAsync(User user, CancellationToken ct)
    {
        var token = GenerateToken(user.Id, "access");
        var descriptor = new TokenDescriptor(user.Id, DomainClock.Instance.UtcNow.Add(_options.AccessTokenLifetime));
        _accessTokens[token] = descriptor;
        _logger.LogDebug("Issued access token for user {UserId} with expiry {Expiry}", user.Id, descriptor.ExpiresAt);
        return Task.FromResult(token);
    }

    public Task<string> CreateRefreshTokenAsync(User user, CancellationToken ct)
    {
        var token = GenerateToken(user.Id, "refresh");
        var descriptor = new TokenDescriptor(user.Id, DomainClock.Instance.UtcNow.Add(_options.RefreshTokenLifetime));
        var userTokens = _refreshTokens.GetOrAdd(user.Id, _ => new ConcurrentDictionary<string, TokenDescriptor>());
        userTokens[token] = descriptor;
        _logger.LogDebug("Issued refresh token for user {UserId} with expiry {Expiry}", user.Id, descriptor.ExpiresAt);
        return Task.FromResult(token);
    }

    public Task<bool> ValidateAccessTokenAsync(string token, CancellationToken ct)
    {
        if (!_accessTokens.TryGetValue(token, out var descriptor))
        {
            return Task.FromResult(false);
        }

        if (descriptor.IsExpired())
        {
            _accessTokens.TryRemove(token, out _);
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken ct)
    {
        if (!_refreshTokens.TryGetValue(userId, out var tokens))
        {
            return Task.FromResult(false);
        }

        if (!tokens.TryGetValue(refreshToken, out var descriptor))
        {
            return Task.FromResult(false);
        }

        if (descriptor.IsExpired())
        {
            tokens.TryRemove(refreshToken, out _);
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public Task RevokeRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken ct)
    {
        if (_refreshTokens.TryGetValue(userId, out var tokens))
        {
            tokens.TryRemove(refreshToken, out _);
        }

        _logger.LogInformation("Revoked refresh token for user {UserId}", userId);
        return Task.CompletedTask;
    }

    private static string GenerateToken(Guid userId, string purpose)
    {
        using var sha256 = SHA256.Create();
        var payload = Encoding.UTF8.GetBytes($"{userId}:{purpose}:{Guid.NewGuid()}:{DomainClock.Instance.UtcNow:o}");
        var hash = sha256.ComputeHash(payload);
        return Convert.ToBase64String(hash);
    }

    private sealed record TokenDescriptor(Guid UserId, DateTime ExpiresAt)
    {
        public bool IsExpired() => DomainClock.Instance.UtcNow >= ExpiresAt;
    }
}