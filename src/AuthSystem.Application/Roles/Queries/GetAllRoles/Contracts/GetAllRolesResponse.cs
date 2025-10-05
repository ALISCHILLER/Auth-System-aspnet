using System;
using System.Collections.Generic;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Roles.Queries.GetAllRoles.Contracts;

public sealed record RoleSummary(
    Guid RoleId,
    string Name,
    string Description,
    bool IsDefault,
    bool IsSystemRole,
    IReadOnlyCollection<PermissionType> Permissions);

public sealed record GetAllRolesResponse(IReadOnlyCollection<RoleSummary> Roles);