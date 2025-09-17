using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد ثبت‌نام کاربر جدید
/// </summary>
public sealed class UserRegisteredEvent : DomainEventBase
{
    public UserRegisteredEvent(Guid userId, string? email, string firstName, string lastName)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid UserId { get; }

    public string? Email { get; }

    public string FirstName { get; }

    public string LastName { get; }
}