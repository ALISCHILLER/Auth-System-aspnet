using System;
using System.Collections.Generic;
using MediatR;
using AuthSystem.Domain.Enums;
using AuthSystem.Application.Roles.Commands.CreateRole.Contracts;
using AuthSystem.Application.Common.Behaviors;

namespace AuthSystem.Application.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(
    string Name,
    string Description,
    bool IsDefault,
    bool IsSystemRole,
    IReadOnlyCollection<PermissionType> Permissions
) : IRequest<CreateRoleResponse>, AuthorizationBehavior, ITransactionalRequest
{
    public PermissionType RequiredPermission => PermissionType.RoleCreate;
}