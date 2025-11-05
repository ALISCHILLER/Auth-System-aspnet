using System;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Application.Contracts.Users;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.LoginUser;

public sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    ITokenService tokenService)
    : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken).ConfigureAwait(false);
        if (user is null || !user.PasswordHash.Verify(request.Password))
        {
            throw new ForbiddenException("Invalid credentials.");
        }

        var accessToken = await tokenService
            .GenerateAccessTokenAsync(user.Id, Array.Empty<string>(), cancellationToken)
            .ConfigureAwait(false);

        var refreshToken = await tokenService
            .GenerateRefreshTokenAsync(user.Id, ipAddress: null, userAgent: null, cancellationToken)
            .ConfigureAwait(false);

        return new LoginUserResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}