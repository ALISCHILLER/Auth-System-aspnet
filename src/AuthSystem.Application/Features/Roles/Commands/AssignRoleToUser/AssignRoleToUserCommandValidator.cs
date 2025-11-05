using FluentValidation;

namespace AuthSystem.Application.Features.Roles.Commands.AssignRoleToUser;

public sealed class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
{
    public AssignRoleToUserCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.RoleId)
            .NotEmpty();
    }
}