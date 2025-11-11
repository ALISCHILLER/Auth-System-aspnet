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
        if (request is not ITransactionalRequest)
        {
            return await next().ConfigureAwait(false);
        }

        var (response, domainEvents) = await unitOfWork
            .ExecuteInTransactionAsync(async ct =>
            {
                var nextResponse = await next().ConfigureAwait(false);
                await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
                var collected = domainEventCollector.DequeueAll();
                return (nextResponse, collected);
            },
            cancellationToken)
            .ConfigureAwait(false);

        if (domainEvents.Count > 0)
        {
            await domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken).ConfigureAwait(false);
        }
        return response;
    }
}