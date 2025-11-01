using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchPendingDomainEventsAsync(CancellationToken ct = default);
}