using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// رویداد بازیابی نقش حذف‌شده
/// </summary>
public sealed class RoleUndeletedEvent : DomainEventBase
{
    public RoleUndeletedEvent(Guid roleId)
    {
        RoleId = roleId;
    }

    public Guid RoleId { get; }
}