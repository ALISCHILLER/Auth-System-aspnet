using System;
using System.Collections.Generic;
using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Factories;

public static class RoleFactory
{
    public static Role Create(Guid id, string name, string description, bool isDefault, bool isSystemRole, IEnumerable<PermissionType> permissions)
    {
        var role = new Role(id, name, description, isDefault, isSystemRole);
        foreach (var permission in permissions)
        {
            role.AddPermission(permission);
        }

        return role;
    }
}