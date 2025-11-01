using System;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// Event emitted whenever a user's primary email address changes.
/// </summary>
public sealed class UserEmailChangedEvent : DomainEventBase
{
    public UserEmailChangedEvent(Guid userId, Email? previousEmail, Email newEmail, bool wasPreviouslyVerified)
    {
        UserId = userId;
        PreviousEmail = previousEmail;
        NewEmail = newEmail;
        WasPreviouslyVerified = wasPreviouslyVerified;
    }

    public Guid UserId { get; }

    public Email? PreviousEmail { get; }

    public Email NewEmail { get; }

    public bool WasPreviouslyVerified { get; }
}