using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// رویداد حذف مجوز از نقش
/// </summary>
public sealed class RolePermissionRemovedEvent : DomainEventBase
{
    public RolePermissionRemovedEvent(Guid roleId, PermissionType permission)
    {
        RoleId = roleId;
        Permission = permission;
    }

    public Guid RoleId { get; }

    public PermissionType Permission { get; }
}