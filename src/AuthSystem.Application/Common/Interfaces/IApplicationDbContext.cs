using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Entities.AuditLog;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    IQueryable<User> Users { get; }
    IQueryable<Role> Roles { get; }
    IQueryable<AuditLog> AuditLogs { get; }
}