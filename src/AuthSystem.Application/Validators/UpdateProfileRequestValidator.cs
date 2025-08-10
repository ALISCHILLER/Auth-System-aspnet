using AuthSystem.Application.DTOs.Requests;
using FluentValidation;

namespace AuthSystem.Application.Validators;

/// <summary>
/// اعتبارسنجی درخواست به‌روزرسانی پروفایل کاربر
/// </summary>
public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.UserName)
            .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.UserName))
            .WithMessage("نام کاربری باید حداقل 3 کاراکتر باشد")
            .MaximumLength(50).WithMessage("نام کاربری نباید بیشتر از 50 کاراکتر باشد");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^09\d{9}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("شماره تلفن باید با 09 شروع شده و 11 رقم باشد");

        RuleFor(x => x.Gender)
            .IsInEnum().When(x => !string.IsNullOrEmpty(x.Gender))
            .WithMessage("جنسیت باید یکی از مقادیر Male، Female یا Other باشد");

        RuleFor(x => x.Culture)
            .Matches(@"^[a-z]{2}(-[A-Z]{2})?$").When(x => !string.IsNullOrEmpty(x.Culture))
            .WithMessage("فرمت زبان نامعتبر است (مثلاً fa-IR یا en-US)");
    }
}