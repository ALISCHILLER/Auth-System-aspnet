using AuthSystem.Application.Features.Audit.Queries.GetSecurityEvents;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AuthSystem.Api.GraphQL;

[ExtendObjectType(typeof(Query))]
public sealed class SecurityEventQuery
{
    [GraphQLType(typeof(SecurityEventResultType))]
    public async Task<PagedResult<SecurityEventDto>> GetSecurityEventsAsync(
        [Service] IMediator mediator,
        int page = 1,
        int pageSize = 25,
        string? tenantId = null,
        SecurityEventType? eventType = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetSecurityEventsQuery(page, pageSize, tenantId, eventType);
        return await mediator.Send(query, cancellationToken).ConfigureAwait(false);
    }
}