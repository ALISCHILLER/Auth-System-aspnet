using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Models;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.Logout;

public sealed class LogoutCommandHandler(
    ITokenService tokenService,
    ISecurityEventPublisher securityEventPublisher) : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            await tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken).ConfigureAwait(false);
        }

        var metadata = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(request.TenantId))
        {
            metadata["tenantId"] = request.TenantId!;
        }

        await securityEventPublisher.PublishAsync(
            new SecurityEventContext(
                SecurityEventType.Logout,
                null,
                null,
                request.TenantId,
                request.IpAddress,
                request.UserAgent,
                "User logged out and refresh token revoked.",
                metadata),
            cancellationToken).ConfigureAwait(false);

        return Unit.Value;
    }
}