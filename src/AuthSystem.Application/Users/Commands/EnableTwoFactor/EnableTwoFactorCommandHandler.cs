using System;
using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Users.Commands.EnableTwoFactor.Contracts;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Application.Users.Commands.EnableTwoFactor;

public sealed class EnableTwoFactorCommandHandler(IUserRepository userRepository)
    : IRequestHandler<EnableTwoFactorCommand, EnableTwoFactorResponse>
{
    public async Task<EnableTwoFactorResponse> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        var secretKey = TwoFactorSecretKey.Generate(request.Issuer);
        user.EnableTwoFactorAuthentication(secretKey);

        await userRepository.UpdateAsync(user, cancellationToken);

        var identifier = request.Email ?? user.Email?.Value ?? user.Username ?? user.Id.ToString();
        var qrCodeUri = secretKey.GenerateUri(identifier);

        return new EnableTwoFactorResponse(secretKey.Value, qrCodeUri);
    }
}