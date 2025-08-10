using AuthSystem.Application.Common;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Events;
using FluentValidation;
using MediatR;

namespace AuthSystem.Application.Features.User.Register;

/// <summary>
/// پردازش‌گر دستور ثبت‌نام کاربر جدید
/// </summary>
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IPasswordService _passwordService;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // بررسی تکراری بودن ایمیل و نام کاربری
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure(AuthStatus.EmailNotConfirmed, "ایمیل قبلاً ثبت شده است");

        if (await _userRepository.ExistsByUsernameAsync(request.UserName, cancellationToken))
            return Result.Failure(AuthStatus.InvalidCredentials, "نام کاربری قبلاً ثبت شده است");

        // هش کردن رمز عبور
        var passwordHash = _passwordService.Hash(request.Password);

        // ساخت کاربر جدید
        var user = User.Create(
            request.UserName,
            request.Email,
            passwordHash,
            request.PhoneNumber ?? string.Empty,
            request.NationalCode ?? string.Empty);

        // تنظیم Culture و ProfileImage
        if (!string.IsNullOrEmpty(request.Culture))
            user.SetCulture(request.Culture);

        if (!string.IsNullOrEmpty(request.ProfileImage))
            user.SetProfileImage(request.ProfileImage);

        // افزودن کاربر به دیتابیس
        await _userRepository.AddAsync(user, cancellationToken);

        // ارسال رویداد ثبت‌نام
        user.AddDomainEvent(new UserRegisteredEvent(user.Id, user.EmailAddress));

        // ارسال ایمیل خوشامدگویی
        await _emailService.SendWelcomeEmailAsync(user.EmailAddress, user.UserName, cancellationToken);

        return Result.Success(AuthStatus.Success, "ثبت‌نام با موفقیت انجام شد");
    }
}