using AuthSystem.Application.DTOs.Requests;
using FluentValidation;

namespace AuthSystem.Application.Validators;

/// <summary>
/// اعتبارسنجی درخواست ورود به سیستم
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("نام کاربری یا ایمیل الزامی است");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("رمز عبور الزامی است");

        RuleFor(x => x.TwoFactorCode)
            .Matches(@"^\d{6}$").When(x => !string.IsNullOrEmpty(x.TwoFactorCode))
            .WithMessage("کد احراز هویت باید 6 رقم باشد");
    }
}