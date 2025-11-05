using System;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// Event emitted when a user's username is changed.
/// </summary>
public sealed class UsernameChangedEvent : DomainEvent
{
    public UsernameChangedEvent(Guid userId, string? previousUsername, string newUsername)
    {
        UserId = userId;
        PreviousUsername = previousUsername;
        NewUsername = newUsername;
    }

    public Guid UserId { get; }

    public string? PreviousUsername { get; }

    public string NewUsername { get; }
}