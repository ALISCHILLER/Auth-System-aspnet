namespace AuthSystem.Application.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchPendingDomainEventsAsync(CancellationToken ct = default);
}