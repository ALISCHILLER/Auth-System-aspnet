using System;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Application.Abstractions;

public interface ITokenService
{
    Task<string> CreateAccessTokenAsync(User user, CancellationToken ct);
    Task<string> CreateRefreshTokenAsync(User user, CancellationToken ct);
    Task<bool> ValidateAccessTokenAsync(string token, CancellationToken ct);
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken ct);
    Task RevokeRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken ct);
}