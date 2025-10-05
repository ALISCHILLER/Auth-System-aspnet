using System;
using System.Linq;
using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Roles.Commands.CreateRole.Contracts;
using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
{
    public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var existingRole = await roleRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingRole is not null)
        {
            throw InvalidUserRoleException.ForDuplicateRole(request.Name);
        }

        var role = new Role(Guid.NewGuid(), request.Name, request.Description, request.IsDefault, request.IsSystemRole);

        foreach (var permission in request.Permissions.Distinct())
        {
            role.AddPermission(permission);
        }

        await roleRepository.AddAsync(role, cancellationToken);

        return new CreateRoleResponse(role.Id, role.Name, role.IsDefault, role.IsSystemRole);
    }
}