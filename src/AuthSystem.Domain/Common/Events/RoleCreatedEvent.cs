using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// رویداد ایجاد نقش جدید
/// </summary>
public class RoleCreatedEvent : DomainEventBase
{
    public Guid RoleId { get; }

    public RoleCreatedEvent(Guid roleId)
    {
        RoleId = roleId;
    }
}