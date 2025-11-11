﻿using AuthSystem.Shared.Contracts.Security;

namespace AuthSystem.Application.Common.Models;

/// <summary>
/// Carries contextual information about a security event that should be persisted and fanned out.
/// </summary>
public sealed record SecurityEventContext(
    SecurityEventType EventType,
    Guid? UserId,
    string? UserName,
    string? TenantId,
    string? IpAddress,
    string? UserAgent,
    string? Description,
    IReadOnlyDictionary<string, string>? Metadata);