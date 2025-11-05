using System.Collections.Generic;
using MediatR;
using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Markers;
using AuthSystem.Application.Contracts.Roles;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Features.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(
    string Name,
    string Description,
    bool IsDefault,
    bool IsSystemRole,
    IReadOnlyCollection<PermissionType> Permissions)
    : IRequest<CreateRoleResponse>, ITransactionalRequest, IRequirePermission
{
    public PermissionType RequiredPermission => PermissionType.RoleCreate;
}