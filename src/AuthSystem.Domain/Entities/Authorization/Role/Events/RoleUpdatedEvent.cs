using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// رویداد به‌روزرسانی نقش
/// </summary>
public sealed class RoleUpdatedEvent : DomainEvent
{
    public RoleUpdatedEvent(Guid roleId, string name, string description)
    {
        RoleId = roleId;
        Name = name;
        Description = description;
    }

    public Guid RoleId { get; }

    public string Name { get; }

    public string Description { get; }
}