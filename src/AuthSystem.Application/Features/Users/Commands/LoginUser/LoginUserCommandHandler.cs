using AuthSystem.Application.Common.Abstractions.Identity;
using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Application.Common.Models;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Shared.Contracts.Security;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.LoginUser;

public sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    ITokenService tokenService,
    ISecurityEventPublisher securityEventPublisher,
    IJitProvisioningService jitProvisioningService)
    : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken).ConfigureAwait(false);
        if (user is null && request.ExternalLogin)
        {
            user = await jitProvisioningService.ProvisionAsync(request.Email, request.TenantId, cancellationToken).ConfigureAwait(false);
        }

        if (user is null)
        {
            throw new ForbiddenException("Invalid credentials.");
        }

        if (!request.ExternalLogin && !user.PasswordHash.Verify(request.Password))
        {
            throw new ForbiddenException("Invalid credentials.");
        }

        var accessToken = await tokenService
            .GenerateAccessTokenAsync(user.Id, Array.Empty<string>(), request.TenantId, cancellationToken)
            .ConfigureAwait(false);

        var refreshToken = await tokenService
            .GenerateRefreshTokenAsync(user.Id, request.IpAddress, request.UserAgent, request.TenantId, cancellationToken)
            .ConfigureAwait(false);

        var response = new LoginUserResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        var metadata = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(user.Email?.Value))
        {
            metadata["email"] = user.Email!.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.TenantId))
        {
            metadata["tenantId"] = request.TenantId!;
        }

        metadata["externalLogin"] = request.ExternalLogin.ToString();

        await securityEventPublisher.PublishAsync(
            new SecurityEventContext(
                SecurityEventType.Login,
                user.Id,
                user.FullName,
                request.TenantId,
                request.IpAddress,
                request.UserAgent,
                "User logged in successfully.",
                metadata),
            cancellationToken).ConfigureAwait(false);

        return response;
    }
}