using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد افزودن نقش به کاربر
/// </summary>
public sealed class UserRoleAddedEvent : DomainEventBase
{
    public UserRoleAddedEvent(Guid userId, Guid roleId, string roleName)
    {
        UserId = userId;
        RoleId = roleId;
        RoleName = roleName;
    }

    public Guid UserId { get; }

    public Guid RoleId { get; }

    public string RoleName { get; }
}