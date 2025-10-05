using FluentValidation;

namespace AuthSystem.Application.Users.Commands.LoginUser;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.EmailOrUsername).NotEmpty();
        RuleFor(command => command.Password).NotEmpty();
    }
}