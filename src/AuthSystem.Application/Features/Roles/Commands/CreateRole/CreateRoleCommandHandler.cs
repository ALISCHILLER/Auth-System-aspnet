using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Application.Contracts.Roles;
using AuthSystem.Domain.Entities.Authorization.Role;
using MediatR;

namespace AuthSystem.Application.Features.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
{
    public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var existingRole = await roleRepository.GetByNameAsync(request.Name, cancellationToken).ConfigureAwait(false);
        if (existingRole is not null)
        {
            throw new ConflictException($"Role '{request.Name}' already exists.");
        }

        var role = new Role(Guid.NewGuid(), request.Name, request.Description, request.IsDefault, request.IsSystemRole);

        foreach (var permission in request.Permissions.Distinct())
        {
            role.AddPermission(permission);
        }

        await roleRepository.AddAsync(role, cancellationToken).ConfigureAwait(false);

        return new CreateRoleResponse
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}