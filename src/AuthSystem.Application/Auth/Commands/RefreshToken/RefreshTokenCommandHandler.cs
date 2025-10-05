using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Auth.Commands.RefreshToken.Contracts;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    ITokenService tokenService)
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw InvalidTokenException.ForEmptyToken();
        }

        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        var isValid = await tokenService.ValidateRefreshTokenAsync(user.Id, request.RefreshToken, cancellationToken);
        if (!isValid)
        {
            throw InvalidTokenException.ForInvalidFormat();
        }

        await tokenService.RevokeRefreshTokenAsync(user.Id, request.RefreshToken, cancellationToken);

        var accessToken = await tokenService.CreateAccessTokenAsync(user, cancellationToken);
        var refreshToken = await tokenService.CreateRefreshTokenAsync(user, cancellationToken);

        await userRepository.UpdateAsync(user, cancellationToken);

        return new RefreshTokenResponse(accessToken, refreshToken);
    }
}