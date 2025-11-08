using System;
using System.Collections.Generic;

namespace AuthSystem.Shared.Contracts.Security;

/// <summary>
/// Represents a security event entry returned to API consumers.
/// </summary>
public sealed record SecurityEventDto
{
    public Guid Id { get; init; }
    public SecurityEventType EventType { get; init; }
    public Guid? UserId { get; init; }
    public string? UserName { get; init; }
    public string? TenantId { get; init; }
    public DateTime OccurredAtUtc { get; init; }
    public string? IpAddress { get; init; }
    public string? UserAgent { get; init; }
    public string? Description { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}