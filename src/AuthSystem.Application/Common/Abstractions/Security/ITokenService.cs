using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Common.Abstractions.Security;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(Guid userId, IEnumerable<string> permissions, CancellationToken cancellationToken);
    Task<string> GenerateRefreshTokenAsync(Guid userId, string? ipAddress, string? userAgent, CancellationToken cancellationToken);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<(bool Success, Guid UserId)> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}