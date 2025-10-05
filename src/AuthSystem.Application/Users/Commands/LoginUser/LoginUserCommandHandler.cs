using System;
using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Users.Commands.LoginUser.Contracts;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Application.Users.Commands.LoginUser;

public sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    ITokenService tokenService,
    IVerificationCodeService verificationCodeService)
    : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await ResolveUserAsync(request.EmailOrUsername, cancellationToken);
        if (user is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        if (user.IsLocked)
        {
            throw new System.UnauthorizedAccessException("حساب کاربری موقتاً قفل است");
        }

        if (!user.PasswordHash.Verify(request.Password))
        {
            user.RegisterLoginFailure("Invalid credentials");
            await userRepository.UpdateAsync(user, cancellationToken);
            throw InvalidUserException.ForInvalidPassword();
        }

        if (user.IsTwoFactorEnabled)
        {
            await verificationCodeService.GenerateTwoFactorCodeAsync(user.Id, cancellationToken);
            return new LoginUserResponse(user.Id, string.Empty, string.Empty, true);
        }

        var ipAddress = TryCreateIpAddress(request.IpAddress);
        var userAgent = TryCreateUserAgent(request.UserAgent);
        user.RegisterLoginSuccess(ipAddress, userAgent);

        await userRepository.UpdateAsync(user, cancellationToken);

        var accessToken = await tokenService.CreateAccessTokenAsync(user, cancellationToken);
        var refreshToken = await tokenService.CreateRefreshTokenAsync(user, cancellationToken);

        return new LoginUserResponse(user.Id, accessToken, refreshToken, false);
    }

    private async Task<User?> ResolveUserAsync(string emailOrUsername, CancellationToken cancellationToken)
    {
        if (Email.IsValidEmail(emailOrUsername))
        {
            var email = Email.Create(emailOrUsername);
            return await userRepository.GetByEmailAsync(email, cancellationToken);
        }

        return await userRepository.GetByUsernameAsync(emailOrUsername, cancellationToken);
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