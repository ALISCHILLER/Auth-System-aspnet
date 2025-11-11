using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Infrastructure.Auth.Models;
using AuthSystem.Infrastructure.Options;
using AuthSystem.Infrastructure.Persistence.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthSystem.Infrastructure.Auth;

internal sealed class JwtTokenService(
    ApplicationDbContext dbContext,
    RefreshTokenHasher hasher,
    IOptions<TokenOptions> tokenOptions) : ITokenService
{
    private readonly TokenOptions _options = tokenOptions.Value;

    public Task<string> GenerateAccessTokenAsync(Guid userId, IEnumerable<string> permissions, string? tenantId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };

        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            claims.Add(new("tenant", tenantId!));
        }

        claims.AddRange(permissions.Select(permission => new Claim("perm", permission)));

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: credentials);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId, string? ipAddress, string? userAgent, string? tenantId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var hashed = hasher.Hash(rawToken);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Hash = hashed,
            Ip = ipAddress,
            UserAgent = userAgent,
            TenantId = tenantId,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_options.RefreshTokenDays)
        };

        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return rawToken;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return false;
        }

        var hash = hasher.Hash(refreshToken);
        var entity = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Hash == hash && x.RevokedAtUtc == null, cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            return false;
        }

        entity.RevokedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    public async Task<(bool Success, Guid UserId, string? TenantId)> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {

        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return (false, Guid.Empty, null);
        }

        var hash = hasher.Hash(refreshToken);
        var entity = await dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.Hash == hash, cancellationToken).ConfigureAwait(false);
        if (entity is null || entity.IsExpired || entity.RevokedAtUtc is not null)
        {
            return (false, Guid.Empty, null);
        }

        return (true, entity.UserId, entity.TenantId);
    }
}