using System;
using System.Collections.Generic;

namespace AuthSystem.Application.Contracts.Users;

public sealed record ScimUserRepresentation
{
    public string Id { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? TenantId { get; init; }
    public bool Active { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = Array.Empty<string>();
}