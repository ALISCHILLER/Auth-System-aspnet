using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Application.Contracts.Users;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    ITokenService tokenService,
    IUserRepository userRepository) : IRequestHandler<RefreshTokenCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var (success, userId, tenantId) = await tokenService
            .ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken)
            .ConfigureAwait(false);

        if (!success || userId == Guid.Empty)
        {
            throw new ForbiddenException("Invalid refresh token.");
        }

        var user = await userRepository.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"User '{userId}' was not found");

        await tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken).ConfigureAwait(false);

        var accessToken = await tokenService
            .GenerateAccessTokenAsync(user.Id, Array.Empty<string>(), tenantId, cancellationToken)
            .ConfigureAwait(false);

        var refreshToken = await tokenService
            .GenerateRefreshTokenAsync(user.Id, ipAddress: null, userAgent: null, tenantId, cancellationToken)
            .ConfigureAwait(false);

        return new LoginUserResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}