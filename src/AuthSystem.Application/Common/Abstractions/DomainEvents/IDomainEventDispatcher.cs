using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Common.Abstractions;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Application.Common.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}