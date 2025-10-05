using System;
using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Users.Commands.LoginUser.Contracts;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Application.Users.Commands.VerifyTwoFactorCode;

public sealed class VerifyTwoFactorCodeCommandHandler(
    IUserRepository userRepository,
    IVerificationCodeService verificationCodeService,
    ITokenService tokenService)
    : IRequestHandler<VerifyTwoFactorCodeCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(VerifyTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        if (!user.IsTwoFactorEnabled || user.TwoFactorSecretKey is null)
        {
            throw new InvalidUserException("احراز هویت دو عاملی برای این کاربر فعال نیست");
        }

        var isValid = await verificationCodeService.ValidateTwoFactorCodeAsync(user.Id, request.Code, cancellationToken);
        if (!isValid)
        {
            user.RegisterLoginFailure("Invalid 2FA code");
            await userRepository.UpdateAsync(user, cancellationToken);
            throw InvalidVerificationCodeException.ForInvalidCode();
        }

        var ipAddress = TryCreateIpAddress(request.IpAddress);
        var userAgent = TryCreateUserAgent(request.UserAgent);
        user.RegisterLoginSuccess(ipAddress, userAgent);

        await userRepository.UpdateAsync(user, cancellationToken);

        var accessToken = await tokenService.CreateAccessTokenAsync(user, cancellationToken);
        var refreshToken = await tokenService.CreateRefreshTokenAsync(user, cancellationToken);

        return new LoginUserResponse(user.Id, accessToken, refreshToken, false);
    }

    private static IpAddress? TryCreateIpAddress(string? ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return null;
        }

        try
        {
            return IpAddress.Create(ip);
        }
        catch
        {
            return null;
        }
    }

    private static UserAgent? TryCreateUserAgent(string? agent)
    {
        if (string.IsNullOrWhiteSpace(agent))
        {
            return null;
        }

        try
        {
            return UserAgent.Create(agent);
        }
        catch
        {
            return UserAgent.Unknown;
        }
    }
}