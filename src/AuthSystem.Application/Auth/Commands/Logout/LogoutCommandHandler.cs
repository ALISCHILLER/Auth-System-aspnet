using MediatR;
using AuthSystem.Application.Abstractions;

namespace AuthSystem.Application.Auth.Commands.Logout;

public sealed class LogoutCommandHandler(ITokenService tokenService)
    : IRequestHandler<LogoutCommand>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            await tokenService.RevokeRefreshTokenAsync(request.UserId, request.RefreshToken, cancellationToken);
        }

        return Unit.Value;
    }
}