using System;
using System.Collections.Concurrent;
using AuthSystem.Domain.Entities.AuditLog;
using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Infrastructure.Persistence.InMemory;

internal sealed class InMemoryDatabase
{
    public ConcurrentDictionary<Guid, User> Users { get; } = new();
    public ConcurrentDictionary<Guid, Role> Roles { get; } = new();
    public ConcurrentDictionary<Guid, AuditLog> AuditLogs { get; } = new();
}