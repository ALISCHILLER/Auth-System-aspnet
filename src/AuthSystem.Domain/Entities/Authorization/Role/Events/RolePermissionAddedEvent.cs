using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// Event emitted when a permission is added to a role.
/// </summary>
public sealed class RolePermissionAddedEvent : DomainEvent
{
    public RolePermissionAddedEvent(Guid roleId, Guid rolePermissionId, PermissionType permissionType)
    {
        RoleId = roleId;
        RolePermissionId = rolePermissionId;
        PermissionType = permissionType;
    }

    public Guid RoleId { get; }
    public Guid RolePermissionId { get; }
    public PermissionType PermissionType { get; }
}