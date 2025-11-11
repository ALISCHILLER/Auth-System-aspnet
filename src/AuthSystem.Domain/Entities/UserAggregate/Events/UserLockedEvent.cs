using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد قفل شدن حساب کاربر
/// </summary>
public sealed class UserLockedEvent : DomainEvent
{
    public UserLockedEvent(Guid userId, DateTime? lockoutEnd)
    {
        UserId = userId;
        LockoutEnd = lockoutEnd;
    }

    public Guid UserId { get; }

    public DateTime? LockoutEnd { get; }
}