using System.Collections.Generic;
using AuthSystem.Domain.Common.Abstractions;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Application.Common.Abstractions.DomainEvents;

public interface IDomainEventCollector
{
    void CollectFrom(params IHasDomainEvents[] aggregates);
    IReadOnlyCollection<IDomainEvent> DequeueAll();
}