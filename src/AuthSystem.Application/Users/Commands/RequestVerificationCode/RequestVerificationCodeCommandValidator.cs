using FluentValidation;

namespace AuthSystem.Application.Users.Commands.RequestVerificationCode;

public sealed class RequestVerificationCodeCommandValidator : AbstractValidator<RequestVerificationCodeCommand>
{
    public RequestVerificationCodeCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.CodeType).IsInEnum();
    }
}