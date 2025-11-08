using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Features.Audit.Queries.GetSecurityEvents;
using AuthSystem.Shared.Contracts;
using AuthSystem.Shared.Contracts.Security;
using AuthSystem.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Api.Controllers;

/// <summary>
/// Provides access to security audit events for operational and compliance purposes.
/// </summary>
[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/audit")]
public sealed class AuditController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves paginated security events with optional filtering.
    /// </summary>
    [HttpGet("security-events")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SecurityEventDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSecurityEvents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? tenantId,
        [FromQuery] SecurityEventType? eventType,
        CancellationToken cancellationToken)
    {
        var query = new GetSecurityEventsQuery(page, pageSize, tenantId, eventType);
        var result = await mediator.Send(query, cancellationToken).ConfigureAwait(false);
        return Ok(ApiResponse<PagedResult<SecurityEventDto>>.Ok(result, HttpContext.TraceIdentifier));
    }
}