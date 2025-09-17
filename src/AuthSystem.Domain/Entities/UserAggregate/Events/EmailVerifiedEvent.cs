using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد تایید ایمیل کاربر
/// </summary>
public sealed class EmailVerifiedEvent : DomainEventBase
{
    public EmailVerifiedEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }

    public Guid UserId { get; }

    public string Email { get; }
}