using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Exceptions;
using MediatR;

namespace AuthSystem.Application.Features.Roles.Commands.AssignRoleToUser;

public sealed class AssignRoleToUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : IRequestHandler<AssignRoleToUserCommand, Unit>
{
    public async Task<Unit> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"User '{request.UserId}' was not found.");

        var role = await roleRepository.GetByIdAsync(request.RoleId, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"Role '{request.RoleId}' was not found.");

        if (!user.HasRole(role.Id))
        {
            user.AddRole(role.Id, role.Name);
            await userRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        return Unit.Value;
    }
}