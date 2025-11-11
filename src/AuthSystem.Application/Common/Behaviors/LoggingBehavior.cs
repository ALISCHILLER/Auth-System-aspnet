using AuthSystem.Application.Common.Abstractions.Diagnostics;
using AuthSystem.Application.Common.Abstractions.Security;
using MediatR;
using System.Diagnostics;
using System.Text.Json;

namespace AuthSystem.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger,
    ICurrentUserService currentUserService,
    ITenantProvider tenantProvider,
    IRequestChannelProvider requestChannelProvider)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false
    };

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = currentUserService.UserId;
        var tenantId = tenantProvider.TenantId;
        var correlationId = Activity.Current?.Id ?? Activity.Current?.TraceId.ToString();
        var channel = requestChannelProvider.Channel;
        int? payloadSize = null;

        try
        {
            payloadSize = JsonSerializer.SerializeToUtf8Bytes(request!, SerializerOptions).Length;
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to determine payload size for {RequestName}", requestName);
        }

        logger.LogInformation(
            "Handling {RequestName} (User: {UserId}, Tenant: {TenantId}, Channel: {Channel}, CorrelationId: {CorrelationId}, PayloadBytes: {PayloadBytes})",
            requestName,
            userId,
            tenantId,
            channel,
            correlationId,
            payloadSize);
        var response = await next().ConfigureAwait(false);
        logger.LogInformation(
            "Handled {RequestName} (User: {UserId}, Tenant: {TenantId}, Channel: {Channel}, CorrelationId: {CorrelationId})",
            requestName,
            userId,
            tenantId,
            channel,
            correlationId);
        return response;
    }
}