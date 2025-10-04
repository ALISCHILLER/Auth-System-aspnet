using System;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role;

/// <summary>
/// Entity representing a permission assigned to a role.
/// </summary>
public class RolePermission : BaseEntity<Guid>
{
    public Guid RoleId { get; private set; }
    public PermissionType PermissionType { get; private set; }

    
    private RolePermission()
    {
    
    }


    public RolePermission(Guid id, Guid roleId, PermissionType permissionType, DateTime? assignedAt = null) : base(id)
    {
        RoleId = roleId;
        PermissionType = permissionType;
        var timestamp = assignedAt?.ToUniversalTime() ?? DomainClock.Instance.UtcNow;
        MarkAsCreated(occurredOn: timestamp);
    }


    public bool IsOfType(PermissionType permissionType) => PermissionType == permissionType;
}
