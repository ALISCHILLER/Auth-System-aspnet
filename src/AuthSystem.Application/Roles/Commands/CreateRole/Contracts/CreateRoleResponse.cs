using System;

namespace AuthSystem.Application.Roles.Commands.CreateRole.Contracts;

public sealed record CreateRoleResponse(Guid RoleId, string Name, bool IsDefault, bool IsSystemRole);