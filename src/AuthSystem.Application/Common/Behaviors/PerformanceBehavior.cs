using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Application.Common.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next().ConfigureAwait(false);
        stopwatch.Stop();
        if (stopwatch.ElapsedMilliseconds > 500)
        {
            logger.LogWarning("Long running request {RequestName} took {Elapsed} ms", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}