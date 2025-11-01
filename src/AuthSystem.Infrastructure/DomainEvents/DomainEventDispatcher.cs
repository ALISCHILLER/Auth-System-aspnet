using System;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Common.Events;
using AuthSystem.Domain.Common.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.DomainEvents;

internal sealed class DomainEventDispatcher(
    IDomainEventCollector collector,
    IPublisher publisher,
    ILogger<DomainEventDispatcher> logger) : IDomainEventDispatcher
{
    public async Task DispatchPendingDomainEventsAsync(CancellationToken ct)
    {
        var events = collector.DrainEvents();
        foreach (var domainEvent in events)
        {
            try
            {
                await PublishAsync(domainEvent, ct).ConfigureAwait(false);
                domainEvent.MarkAsPublished();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to dispatch domain event {EventId} ({EventType})", domainEvent.EventId, domainEvent.GetType().Name);
                throw;
            }
        }

        if (events.Count > 0)
        {
            logger.LogDebug("Dispatched {Count} domain events", events.Count);
        }
    }

    private Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct)
    {
        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
        var notification = Activator.CreateInstance(notificationType, domainEvent)
            ?? throw new InvalidOperationException($"Failed to create domain event notification for {domainEvent.GetType().Name}");

        return publisher.Publish((INotification)notification, ct);
    }
}