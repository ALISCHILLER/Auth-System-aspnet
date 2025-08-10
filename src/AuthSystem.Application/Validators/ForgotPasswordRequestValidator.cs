using AuthSystem.Application.DTOs.Requests;
using FluentValidation;

namespace AuthSystem.Application.Validators;

/// <summary>
/// اعتبارسنجی درخواست بازنشانی رمز عبور
/// </summary>
public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("ایمیل الزامی است")
            .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است");
    }
}