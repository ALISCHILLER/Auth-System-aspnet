using System;
using System.Linq;
using MediatR;
using AuthSystem.Application.AuditLogs.Queries.GetAuditLogsByUser.Contracts;
using AuthSystem.Application.Common.Interfaces;

namespace AuthSystem.Application.AuditLogs.Queries.GetAuditLogsByUser;

public sealed class GetAuditLogsByUserQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetAuditLogsByUserQuery, GetAuditLogsByUserResponse>
{
    public Task<GetAuditLogsByUserResponse> Handle(GetAuditLogsByUserQuery request, CancellationToken cancellationToken)
    {
        var limit = Math.Clamp(request.Limit, 1, 200);

        var entries = dbContext.AuditLogs
            .SelectMany(log => log.Entries)
            .Where(entry => entry.UserId == request.UserId)
            .OrderByDescending(entry => entry.Timestamp)
            .Take(limit)
            .Select(entry => new AuditLogEntryDto(
                entry.Id,
                entry.Timestamp,
                entry.Action,
                entry.Description,
                entry.LogLevel,
                entry.IpAddress.Value,
                entry.UserAgent.Value))
            .ToArray();

        return Task.FromResult(new GetAuditLogsByUserResponse(request.UserId, entries));
    }
}