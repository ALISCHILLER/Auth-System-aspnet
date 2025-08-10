using AuthSystem.Application.DTOs.Requests;
using AuthSystem.Application.Interfaces;
using FluentValidation;
using System.Threading.Tasks;

namespace AuthSystem.Application.Validators;

/// <summary>
/// اعتبارسنجی درخواست ثبت‌نام کاربر جدید
/// </summary>
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    private readonly IUserRepository _userRepository;

    public RegisterRequestValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("نام کاربری الزامی است")
            .MinimumLength(3).WithMessage("نام کاربری باید حداقل 3 کاراکتر باشد")
            .MaximumLength(50).WithMessage("نام کاربری نباید بیشتر از 50 کاراکتر باشد")
            .MustAsync(BeUniqueUsername).WithMessage("نام کاربری قبلاً ثبت شده است");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("ایمیل الزامی است")
            .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است")
            .MustAsync(BeUniqueEmail).WithMessage("ایمیل قبلاً ثبت شده است");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("رمز عبور الزامی است")
            .MinimumLength(8).WithMessage("رمز عبور باید حداقل 8 کاراکتر باشد")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("رمز عبور باید شامل حرف بزرگ، حرف کوچک، عدد و یک نماد خاص باشد");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("رمز عبور و تکرار آن مطابقت ندارند");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^09\d{9}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("شماره تلفن باید با 09 شروع شده و 11 رقم باشد");

        RuleFor(x => x.NationalCode)
            .Matches(@"^\d{10}$").When(x => !string.IsNullOrEmpty(x.NationalCode))
            .WithMessage("کد ملی باید دقیقاً 10 رقم باشد");

        RuleFor(x => x.Culture)
            .Matches(@"^[a-z]{2}(-[A-Z]{2})?$").When(x => !string.IsNullOrEmpty(x.Culture))
            .WithMessage("فرمت زبان نامعتبر است (مثلاً fa-IR یا en-US)");
    }


    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        return !await _userRepository.ExistsByUsernameAsync(username, cancellationToken);
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return !await _userRepository.ExistsByEmailAsync(email, cancellationToken);
    }
    private bool BeValidBase64Image(string base64)
    {
        if (!base64.StartsWith("data:image/"))
            return false;
        try
        {
            var bytes = Convert.FromBase64String(base64.Split(',')[1]);
            return bytes.Length <= 2 * 1024 * 1024; // حداکثر 2 مگابایت
        }
        catch
        {
            return false;
        }
    }
}