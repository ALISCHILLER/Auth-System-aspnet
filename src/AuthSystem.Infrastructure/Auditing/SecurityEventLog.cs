using System;

namespace AuthSystem.Infrastructure.Auditing;

internal sealed class SecurityEventLog
{
    public Guid Id { get; set; }

    public string EventType { get; set; } = string.Empty;

    public Guid? UserId { get; set; }

    public string? UserName { get; set; }

    public string? TenantId { get; set; }

    public DateTime OccurredAtUtc { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? Description { get; set; }

    public string? MetadataJson { get; set; }
}