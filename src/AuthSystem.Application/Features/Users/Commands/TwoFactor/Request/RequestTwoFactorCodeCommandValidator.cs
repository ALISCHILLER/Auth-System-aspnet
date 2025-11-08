using FluentValidation;

namespace AuthSystem.Application.Features.Users.Commands.TwoFactor.Request;

public sealed class RequestTwoFactorCodeCommandValidator : AbstractValidator<RequestTwoFactorCodeCommand>
{
    public RequestTwoFactorCodeCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}