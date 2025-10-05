using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Roles.Commands.AssignRoleToUser;

public sealed class AssignRoleToUserCommandHandler(
    IRoleRepository roleRepository,
    IUserRepository userRepository)
    : IRequestHandler<AssignRoleToUserCommand>
{
    public async Task<Unit> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
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

        var username = user.Username ?? user.Email?.Value ?? user.FullName;
        role.AddUserToRole(user.Id, username);
        user.AddRole(role.Id, role.Name);

        await roleRepository.UpdateAsync(role, cancellationToken);
        await userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}