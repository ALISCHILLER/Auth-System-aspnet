using AuthSystem.Application.Common.Abstractions.Diagnostics;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;


namespace AuthSystem.Application.Common.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
    ICurrentUserService currentUserService,
    ITenantProvider tenantProvider,
    IRequestChannelProvider requestChannelProvider,
    IOptions<PipelineLoggingOptions> options)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next().ConfigureAwait(false);
        stopwatch.Stop();
        var threshold = options.Value.SlowRequestThresholdMilliseconds;
        if (stopwatch.ElapsedMilliseconds > threshold)
        {
            var userId = currentUserService.UserId;
            var tenantId = tenantProvider.TenantId;
            var channel = requestChannelProvider.Channel;
            var correlationId = Activity.Current?.Id ?? Activity.Current?.TraceId.ToString();

            logger.LogWarning(
                "Slow request {RequestName} took {Elapsed} ms (Threshold: {Threshold} ms, User: {UserId}, Tenant: {TenantId}, Channel: {Channel}, CorrelationId: {CorrelationId})",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds,
                threshold,
                userId,
                tenantId,
                channel,
                correlationId);
        }

        return response;
    }
}