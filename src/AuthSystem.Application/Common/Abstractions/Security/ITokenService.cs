using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Common.Abstractions.Security;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(Guid userId, IEnumerable<string> permissions, string? tenantId, CancellationToken cancellationToken);
    Task<string> GenerateRefreshTokenAsync(Guid userId, string? ipAddress, string? userAgent, string? tenantId, CancellationToken cancellationToken);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<(bool Success, Guid UserId, string? TenantId)> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}