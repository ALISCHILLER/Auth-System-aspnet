using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Roles.Commands.RemoveRoleFromUser;

public sealed class RemoveRoleFromUserCommandHandler(
    IRoleRepository roleRepository,
    IUserRepository userRepository)
    : IRequestHandler<RemoveRoleFromUserCommand>
{
    public async Task<Unit> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
        {
            throw InvalidUserRoleException.ForNonExistentRole(request.RoleId.ToString());
        }

        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        role.RemoveUserFromRole(user.Id);
        user.RemoveRole(role.Id);

        await roleRepository.UpdateAsync(role, cancellationToken);
        await userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}