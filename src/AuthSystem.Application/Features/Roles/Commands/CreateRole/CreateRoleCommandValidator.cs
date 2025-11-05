using FluentValidation;

namespace AuthSystem.Application.Features.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(command => command.Description)
            .MaximumLength(256);
    }
}