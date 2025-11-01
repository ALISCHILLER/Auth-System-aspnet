using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Infrastructure.DomainEvents;

internal sealed class InMemoryDomainEventCollector : IDomainEventCollector
{
    private readonly ConcurrentQueue<IDomainEvent> _events = new();

    public void CollectFromAggregate(AggregateRoot<Guid> aggregate)
    {
        if (aggregate is null)
        {
            throw new ArgumentNullException(nameof(aggregate));
        }

        if (!aggregate.HasDomainEvents())
        {
            return;
        }

        foreach (var domainEvent in aggregate.DomainEvents)
        {
            _events.Enqueue(domainEvent);
        }

        aggregate.ClearDomainEvents();
    }

    public IReadOnlyCollection<IDomainEvent> DrainEvents()
    {
        var drained = new List<IDomainEvent>();
        while (_events.TryDequeue(out var domainEvent))
        {
            drained.Add(domainEvent);
        }

        return drained.AsReadOnly();
    }
}