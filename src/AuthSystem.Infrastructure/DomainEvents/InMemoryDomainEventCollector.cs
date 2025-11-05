using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AuthSystem.Application.Common.Abstractions.DomainEvents;
using AuthSystem.Domain.Common.Abstractions;

namespace AuthSystem.Infrastructure.DomainEvents;

internal sealed class InMemoryDomainEventCollector : IDomainEventCollector
{
    private readonly ConcurrentQueue<IDomainEvent> _events = new();

    public void CollectFrom(params IHasDomainEvents[] aggregates)
    {
        foreach (var aggregate in aggregates)
        {
            if (aggregate is null)
            {
                continue;
            }

            var pendingEvents = aggregate.DequeueDomainEvents();
            foreach (var domainEvent in pendingEvents)
            {
                _events.Enqueue(domainEvent);
            }
        }
    }

    public IReadOnlyCollection<IDomainEvent> DequeueAll()
    {
        var drained = new List<IDomainEvent>();
        while (_events.TryDequeue(out var domainEvent))
        {
            drained.Add(domainEvent);
        }

        return drained;
    }
}