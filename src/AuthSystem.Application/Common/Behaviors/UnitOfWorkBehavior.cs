using MediatR;
using AuthSystem.Application.Abstractions;

namespace AuthSystem.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    IDomainEventDispatcher domainEventDispatcher)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ITransactionalRequest)
        {
            return await next();
        }

        await unitOfWork.BeginAsync(cancellationToken);
        try
        {
            var response = await next();
            await unitOfWork.CommitAsync(cancellationToken);
            await domainEventDispatcher.DispatchPendingDomainEventsAsync(cancellationToken);
            return response;
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}