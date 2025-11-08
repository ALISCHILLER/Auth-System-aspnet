using System;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Application.Features.Audit.Queries.GetSecurityEvents;
using AuthSystem.Shared.Contracts.Security;
using AuthSystem.Shared.DTOs;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace AuthSystem.Api.Grpc;

public sealed class SecurityEventsGrpcService(IMediator mediator) : SecurityEvents.SecurityEventsBase
{
    public override async Task<SecurityEventsResponse> ListSecurityEvents(SecurityEventsRequest request, ServerCallContext context)
    {
        var eventType = Enum.TryParse<SecurityEventType>(request.EventType, ignoreCase: true, out var parsed)
            ? parsed
            : null as SecurityEventType?;

        var query = new GetSecurityEventsQuery(
            request.Page <= 0 ? 1 : request.Page,
            request.PageSize <= 0 ? 25 : request.PageSize,
            string.IsNullOrWhiteSpace(request.TenantId) ? null : request.TenantId,
            eventType);

        var result = await mediator.Send(query, context.CancellationToken).ConfigureAwait(false);

        var response = new SecurityEventsResponse
        {
            Page = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };

        response.Items.AddRange(result.Items.Select(MapEvent));

        return response;
    }

    private static SecurityEvent MapEvent(SecurityEventDto dto)
    {
        var evt = new SecurityEvent
        {
            Id = dto.Id.ToString(),
            EventType = dto.EventType.ToString(),
            UserId = dto.UserId?.ToString() ?? string.Empty,
            UserName = dto.UserName ?? string.Empty,
            TenantId = dto.TenantId ?? string.Empty,
            OccurredAtUtc = Timestamp.FromDateTime(dto.OccurredAtUtc.ToUniversalTime()),
            IpAddress = dto.IpAddress ?? string.Empty,
            UserAgent = dto.UserAgent ?? string.Empty,
            Description = dto.Description ?? string.Empty
        };

        if (dto.Metadata is not null)
        {
            foreach (var kvp in dto.Metadata)
            {
                evt.Metadata[kvp.Key] = kvp.Value;
            }
        }

        return evt;
    }
}