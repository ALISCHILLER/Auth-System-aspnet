using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد باز شدن قفل حساب کاربر
/// </summary>
public sealed class UserUnlockedEvent : DomainEventBase
{
    public UserUnlockedEvent(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}