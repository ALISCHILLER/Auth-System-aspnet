using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Shared.Contracts.Security;
using AuthSystem.Shared.DTOs;
using MediatR;

namespace AuthSystem.Application.Features.Audit.Queries.GetSecurityEvents;

public sealed record GetSecurityEventsQuery(
    int Page = 1,
    int PageSize = 25,
    string? TenantId = null,
    SecurityEventType? EventType = null)
    : IRequest<PagedResult<SecurityEventDto>>, IRequirePermission
{
    public PermissionType RequiredPermission => PermissionType.SecurityManagement;
}