using AuthSystem.Application.Common.Abstractions.Identity;
using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Application.Common.Models;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Shared.Contracts.Security;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.Logout;

public sealed class LogoutCommandHandler(
    IRefreshTokenService refreshTokenService,
    ISecurityEventPublisher securityEventPublisher)
    : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await refreshTokenService.RevokeAsync(request.RefreshToken, cancellationToken).ConfigureAwait(false);

        await securityEventPublisher.PublishAsync(
            new SecurityEventContext(
                SecurityEventType.Logout,
                request.UserId,
                request.UserName,
                request.TenantId,
                request.IpAddress,
                request.UserAgent,
                "User logged out and refresh token revoked.",
                new Dictionary<string, string>
                {
                    ["refreshTokenId"] = request.RefreshToken
                }),
            cancellationToken).ConfigureAwait(false);
    }
}