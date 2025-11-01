using System.Linq;
using AuthSystem.Application.Common.Interfaces;
using AuthSystem.Domain.Entities.AuditLog;
using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Infrastructure.Persistence.InMemory;

internal sealed class InMemoryApplicationDbContext(InMemoryDatabase database) : IApplicationDbContext
{
    public IQueryable<User> Users => database.Users.Values.AsQueryable();
    public IQueryable<Role> Roles => database.Roles.Values.AsQueryable();
    public IQueryable<AuditLog> AuditLogs => database.AuditLogs.Values.AsQueryable();
}