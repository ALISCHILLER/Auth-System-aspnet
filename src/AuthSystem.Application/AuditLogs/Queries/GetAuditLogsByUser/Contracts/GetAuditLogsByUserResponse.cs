using System;
using System.Collections.Generic;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.AuditLogs.Queries.GetAuditLogsByUser.Contracts;

public sealed record AuditLogEntryDto(
    Guid EntryId,
    DateTime Timestamp,
    string Action,
    string Description,
    AuditLogLevel Level,
    string IpAddress,
    string UserAgent);

public sealed record GetAuditLogsByUserResponse(Guid UserId, IReadOnlyCollection<AuditLogEntryDto> Entries);