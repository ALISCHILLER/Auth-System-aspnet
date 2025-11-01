using System;
using System.Collections.Generic;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Application.Abstractions;

public interface IDomainEventCollector
{
    void CollectFromAggregate(AggregateRoot<Guid> aggregate);
    IReadOnlyCollection<IDomainEvent> DrainEvents();
}