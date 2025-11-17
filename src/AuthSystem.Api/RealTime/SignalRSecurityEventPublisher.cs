using AuthSystem.Api.Hubs;
using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Application.Common.Models;
using AuthSystem.Infrastructure.SecurityEvents;
using AuthSystem.Shared.Contracts.Security;
using AuthSystem.Shared.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace AuthSystem.Api.RealTime;

internal sealed class SignalRSecurityEventPublisher : ISecurityEventPublisher
{
    private readonly ISecurityEventPublisher _inner;
    private readonly IHubContext<SecurityEventsHub> _hubContext;

    public SignalRSecurityEventPublisher(ISecurityEventPublisher inner, IHubContext<SecurityEventsHub> hubContext)
        => (_inner, _hubContext) = (inner, hubContext);

    public async Task PublishAsync(SecurityEventContext context, CancellationToken cancellationToken)
    {
        await _inner.PublishAsync(context, cancellationToken).ConfigureAwait(false);

        var metadata = context.Metadata is null
            ? null
            : new Dictionary<string, string>(context.Metadata);

        var dto = new SecurityEventDto
        {
            Id = Guid.NewGuid(),
            EventType = context.EventType,
            UserId = context.UserId,
            UserName = context.UserName,
            TenantId = context.TenantId,
            OccurredAtUtc = DateTime.UtcNow,
            IpAddress = context.IpAddress,
            UserAgent = context.UserAgent,
            Description = context.Description,
            Metadata = metadata
        };

        await _hubContext.Clients.All.SendAsync("securityEvent", dto, cancellationToken).ConfigureAwait(false);
    }
}