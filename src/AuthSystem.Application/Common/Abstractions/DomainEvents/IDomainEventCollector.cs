using AuthSystem.Domain.Common.Abstractions;

namespace AuthSystem.Application.Common.Abstractions.DomainEvents;

public interface IDomainEventCollector
{
    void CollectFrom(params IHasDomainEvents[] aggregates);
    IReadOnlyCollection<IDomainEvent> DequeueAll();
}