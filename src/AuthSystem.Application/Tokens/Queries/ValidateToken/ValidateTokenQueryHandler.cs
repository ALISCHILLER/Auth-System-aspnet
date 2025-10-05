using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Tokens.Queries.ValidateToken.Contracts;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Tokens.Queries.ValidateToken;

public sealed class ValidateTokenQueryHandler(ITokenService tokenService)
    : IRequestHandler<ValidateTokenQuery, ValidateTokenResponse>
{
    public async Task<ValidateTokenResponse> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            throw InvalidTokenException.ForEmptyToken();
        }

        bool isValid;
        if (request.IsAccessToken)
        {
            isValid = await tokenService.ValidateAccessTokenAsync(request.Token, cancellationToken);
        }
        else
        {
            if (!request.UserId.HasValue)
            {
                throw new InvalidTokenException("برای اعتبارسنجی توکن رفرش نیاز به شناسه کاربر است");
            }

            isValid = await tokenService.ValidateRefreshTokenAsync(request.UserId.Value, request.Token, cancellationToken);
        }

        return new ValidateTokenResponse(isValid);
    }
}