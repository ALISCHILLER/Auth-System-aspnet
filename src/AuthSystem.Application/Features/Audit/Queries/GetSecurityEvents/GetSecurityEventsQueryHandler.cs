using AuthSystem.Application.Common.Abstractions.Monitoring;
using MediatR;

namespace AuthSystem.Application.Features.Audit.Queries.GetSecurityEvents;

public sealed class GetSecurityEventsQueryHandler(ISecurityEventReader reader)
    : IRequestHandler<GetSecurityEventsQuery, PagedResult<SecurityEventDto>>
{
    public Task<PagedResult<SecurityEventDto>> Handle(GetSecurityEventsQuery request, CancellationToken cancellationToken)
    {
        var pagedRequest = new PagedRequest(request.Page, request.PageSize);
        if (!string.IsNullOrWhiteSpace(request.TenantId))
        {
            pagedRequest.Filters["tenantId"] = request.TenantId!;
        }

        if (request.EventType is not null)
        {
            pagedRequest.Filters["eventType"] = request.EventType.Value.ToString();
        }

        return reader.GetAsync(pagedRequest, cancellationToken);
    }
}