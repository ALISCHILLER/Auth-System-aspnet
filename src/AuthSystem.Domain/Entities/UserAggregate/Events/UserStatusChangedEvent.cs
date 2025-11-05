using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد تغییر وضعیت کاربر
/// </summary>
public sealed class UserStatusChangedEvent : DomainEvent
{
    public UserStatusChangedEvent(Guid userId, UserStatus previousStatus, UserStatus newStatus)
    {
        UserId = userId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
    }

    public Guid UserId { get; }

    public UserStatus PreviousStatus { get; }

    public UserStatus NewStatus { get; }
}