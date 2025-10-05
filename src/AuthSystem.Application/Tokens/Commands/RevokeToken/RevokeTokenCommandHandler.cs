using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Tokens.Commands.RevokeToken;

public sealed class RevokeTokenCommandHandler(ITokenService tokenService)
    : IRequestHandler<RevokeTokenCommand>
{
    public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw InvalidTokenException.ForEmptyToken();
        }

        await tokenService.RevokeRefreshTokenAsync(request.UserId, request.RefreshToken, cancellationToken);
        return Unit.Value;
    }
}