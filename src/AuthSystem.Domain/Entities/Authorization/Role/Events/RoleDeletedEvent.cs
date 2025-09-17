using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// رویداد حذف نقش
/// </summary>
public sealed class RoleDeletedEvent : DomainEventBase
{
    public RoleDeletedEvent(Guid roleId)
    {
        RoleId = roleId;
    }

    public Guid RoleId { get; }
}