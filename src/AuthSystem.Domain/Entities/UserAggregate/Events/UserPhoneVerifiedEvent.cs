using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// Event emitted when a user's phone number is verified.
/// </summary>
public sealed class UserPhoneVerifiedEvent : DomainEvent
{
    public UserPhoneVerifiedEvent(Guid userId, PhoneNumber phoneNumber)
    {
        UserId = userId;
        PhoneNumber = phoneNumber;
    }

    public Guid UserId { get; }

    public PhoneNumber PhoneNumber { get; }
}