using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// رویداد ایجاد نقش جدید
/// </summary>
public sealed class RoleCreatedEvent : DomainEvent
{
    public RoleCreatedEvent(Guid roleId, string name, string description, bool isDefault, bool isSystemRole)
    {
        RoleId = roleId;
        Name = name;
        Description = description;
        IsDefault = isDefault;
        IsSystemRole = isSystemRole;
    }

    public Guid RoleId { get; }

    public string Name { get; }

    public string Description { get; }

    public bool IsDefault { get; }

    public bool IsSystemRole { get; }
}