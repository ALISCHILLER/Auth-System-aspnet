using System;
using System.Collections.Generic;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Events;

/// <summary>
/// Base implementation for domain events.
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    public DateTime OccurredOn { get; } = DomainClock.Instance.UtcNow;
    public Guid EventId { get; } = Guid.NewGuid();

    public Dictionary<string, object?> Metadata { get; } = new();
   

    public virtual bool IsAsync => false;

    public bool IsPublished { get; private set; }

    public void MarkAsPublished() => IsPublished = true;

    public void AddMetadata(string key, object? value) => Metadata[key] = value;
}