using AuthSystem.Domain.Common.Abstractions;

namespace AuthSystem.Application.Common.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}