using System;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// Event emitted when the failed access counter of a user is reset.
/// </summary>
public sealed class UserAccessFailedCountResetEvent : DomainEvent
{
    public UserAccessFailedCountResetEvent(Guid userId, int previousFailedCount)
    {
        UserId = userId;
        PreviousFailedCount = previousFailedCount;
    }

    public Guid UserId { get; }

    public int PreviousFailedCount { get; }
}