using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Application.Common.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();
        logger.LogDebug("{Request} executed in {Elapsed} ms", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
        return response;
    }
}