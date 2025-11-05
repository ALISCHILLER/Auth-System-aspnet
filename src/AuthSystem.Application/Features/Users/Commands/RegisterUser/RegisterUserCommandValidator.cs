using FluentValidation;

namespace AuthSystem.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(100);

        RuleFor(command => command.FirstName)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(command => command.LastName)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(command => command.PhoneNumber)
            .MaximumLength(32);
    }
}