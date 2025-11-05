using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.DomainEvents;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Markers;
using MediatR;

namespace AuthSystem.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    IDomainEventCollector domainEventCollector,
    IDomainEventDispatcher domainEventDispatcher)
    : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next().ConfigureAwait(false);

        if (request is ITransactionalRequest)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            var domainEvents = domainEventCollector.DequeueAll();
            if (domainEvents.Count > 0)
            {
                await domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken).ConfigureAwait(false);
            }
        }
        return response;
    }
}