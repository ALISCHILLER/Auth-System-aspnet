using AuthSystem.Application.Common;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Enums;
using MediatR;

namespace AuthSystem.Application.Features.User.ChangePassword;

/// <summary>
/// پردازش‌گر دستور تغییر رمز عبور
/// </summary>
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _emailService = emailService;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            return Result.Failure(AuthStatus.UserNotFound, "کاربر یافت نشد");

        if (!_passwordService.Verify(request.CurrentPassword, user.PasswordHash))
            return Result.Failure(AuthStatus.InvalidCredentials, "رمز عبور فعلی نادرست است");

        var newHash = _passwordService.Hash(request.NewPassword);
        user.ChangePassword(newHash);

        await _userRepository.UpdateAsync(user, cancellationToken);

        await _emailService.SendPasswordChangedEmailAsync(user.EmailAddress, user.UserName, cancellationToken);

        return Result.Success(AuthStatus.Success, "رمز عبور با موفقیت تغییر کرد");
    }
}