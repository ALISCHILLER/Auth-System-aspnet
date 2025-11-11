using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Application.Common.Models;
using AuthSystem.Shared.Contracts.Security;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.Logout;

public sealed class LogoutCommandHandler(
    ITokenService tokenService,
    ICurrentUserService currentUserService,
    ISecurityEventPublisher securityEventPublisher)
    : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken).ConfigureAwait(false);

        var userId = request.UserId ?? currentUserService.UserId;
        var userName = string.IsNullOrWhiteSpace(request.UserName) ? null : request.UserName;

        var metadata = new Dictionary<string, string>
        {
            ["refreshTokenId"] = request.RefreshToken
        };

        if (!string.IsNullOrWhiteSpace(request.TenantId))
        {
            metadata["tenantId"] = request.TenantId!;
        }

        if (!string.IsNullOrWhiteSpace(request.IpAddress))
        {
            metadata["ipAddress"] = request.IpAddress!;
        }

        await securityEventPublisher.PublishAsync(
            new SecurityEventContext(
                SecurityEventType.Logout,
                userId,
                userName,
                request.TenantId,
                request.IpAddress,
                request.UserAgent,
                "User logged out and refresh token revoked.",
                  metadata),
            cancellationToken).ConfigureAwait(false);
        return Unit.Value;
    }
}