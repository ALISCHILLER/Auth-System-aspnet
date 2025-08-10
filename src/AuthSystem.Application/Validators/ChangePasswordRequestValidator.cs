using AuthSystem.Application.DTOs.Requests;
using FluentValidation;

namespace AuthSystem.Application.Validators;

/// <summary>
/// اعتبارسنجی درخواست تغییر رمز عبور
/// </summary>
public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("رمز عبور فعلی الزامی است");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("رمز عبور جدید الزامی است")
            .MinimumLength(8).WithMessage("رمز عبور جدید باید حداقل 8 کاراکتر باشد")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("رمز عبور جدید باید شامل حرف بزرگ، حرف کوچک، عدد و یک نماد خاص باشد");

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage("رمز عبور جدید و تکرار آن مطابقت ندارند");
    }
}