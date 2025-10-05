using FluentValidation;

namespace AuthSystem.Application.Users.Commands.EnableTwoFactor;

public sealed class EnableTwoFactorCommandValidator : AbstractValidator<EnableTwoFactorCommand>
{
    public EnableTwoFactorCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Issuer).NotEmpty().MaximumLength(128);

        RuleFor(command => command.Email)
            .Cascade(CascadeMode.Stop)
            .EmailAddress()
            .MaximumLength(256)
            .When(command => !string.IsNullOrWhiteSpace(command.Email));
    }
}