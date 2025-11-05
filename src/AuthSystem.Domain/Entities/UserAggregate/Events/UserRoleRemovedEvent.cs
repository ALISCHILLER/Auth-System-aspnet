using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد حذف نقش از کاربر
/// </summary>
public sealed class UserRoleRemovedEvent : DomainEvent
{
    public UserRoleRemovedEvent(Guid userId, Guid roleId, string roleName)
    {
        UserId = userId;
        RoleId = roleId;
        RoleName = roleName;
    }

    public Guid UserId { get; }

    public Guid RoleId { get; }

    public string RoleName { get; }
}