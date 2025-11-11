using AuthSystem.Application.Common.Abstractions.DomainEvents;
using AuthSystem.Application.Common.Events;
using AuthSystem.Domain.Common.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.DomainEvents;

internal sealed class DomainEventDispatcher(
    IPublisher publisher,
    ILogger<DomainEventDispatcher> logger) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in events)
        {
            try
            {
                var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
                var notification = Activator.CreateInstance(notificationType, domainEvent)
                    ?? throw new InvalidOperationException($"Unable to create notification for {domainEvent.GetType().Name}");

                await publisher.Publish((INotification)notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to dispatch domain event {EventId} ({EventType})", domainEvent.EventId, domainEvent.GetType().Name);
                throw;
            }
        }
    }
}