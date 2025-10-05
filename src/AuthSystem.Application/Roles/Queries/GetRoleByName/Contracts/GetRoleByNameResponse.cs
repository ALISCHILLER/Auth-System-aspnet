using System;
using System.Collections.Generic;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Roles.Queries.GetRoleByName.Contracts;

public sealed record GetRoleByNameResponse(
    Guid RoleId,
    string Name,
    string Description,
    bool IsDefault,
    bool IsSystemRole,
    IReadOnlyCollection<PermissionType> Permissions);