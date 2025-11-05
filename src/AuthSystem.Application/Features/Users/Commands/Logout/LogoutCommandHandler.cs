using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Security;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.Logout;

public sealed class LogoutCommandHandler(ITokenService tokenService) : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            await tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken).ConfigureAwait(false);
        }

        return Unit.Value;
    }
}