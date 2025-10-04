using System;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// Event emitted when a user's password is changed.
/// </summary>
public sealed class UserPasswordChangedEvent : DomainEventBase
{
    public UserPasswordChangedEvent(Guid userId, PasswordHash passwordHash)
    {
        UserId = userId;
        PasswordHash = passwordHash;
    }

    public Guid UserId { get; }
    public PasswordHash PasswordHash { get; }
}