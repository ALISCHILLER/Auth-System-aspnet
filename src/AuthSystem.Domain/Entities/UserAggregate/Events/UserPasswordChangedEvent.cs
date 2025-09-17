using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد تغییر رمز عبور کاربر
/// </summary>
public sealed class UserPasswordChangedEvent : DomainEventBase
{
    public UserPasswordChangedEvent(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}