using System;
using MediatR;
using AuthSystem.Application.AuditLogs.Queries.GetAuditLogsByUser.Contracts;

namespace AuthSystem.Application.AuditLogs.Queries.GetAuditLogsByUser;

public sealed record GetAuditLogsByUserQuery(Guid UserId, int Limit = 50) : IRequest<GetAuditLogsByUserResponse>;