using FluentValidation;

namespace AuthSystem.Application.Features.Users.Commands.TwoFactor.Verify;

public sealed class VerifyTwoFactorCodeCommandValidator : AbstractValidator<VerifyTwoFactorCodeCommand>
{
    public VerifyTwoFactorCodeCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Code)
            .NotEmpty()
            .Length(4, 12);
    }
}