using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد تلاش ناموفق برای ورود کاربر
/// </summary>
public sealed class UserLoginFailedEvent : DomainEvent
{
    public UserLoginFailedEvent(Guid userId, string reason, int failedAttempts)
    {
        UserId = userId;
        Reason = reason;
        FailedAttempts = failedAttempts;
    }

    public Guid UserId { get; }

    public string Reason { get; }

    public int FailedAttempts { get; }
}