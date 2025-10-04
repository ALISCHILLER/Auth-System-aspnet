using System;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// رویداد حذف مجوز از نقش
/// </summary>
public sealed class RolePermissionRemovedEvent : DomainEventBase
{
    public RolePermissionRemovedEvent(Guid roleId, Guid rolePermissionId, PermissionType permissionType)
    {
        RoleId = roleId;
        RolePermissionId = rolePermissionId;
        PermissionType = permissionType;
    }

    public Guid RoleId { get; }

    public Guid RolePermissionId { get; }
    public PermissionType PermissionType { get; }
}