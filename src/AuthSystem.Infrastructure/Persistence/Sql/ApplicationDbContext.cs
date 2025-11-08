using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.ValueObjects;
using AuthSystem.Infrastructure.Auditing;
using AuthSystem.Infrastructure.Auth.Models;
using AuthSystem.Infrastructure.Verification.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Infrastructure.Persistence.Sql;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();
    public DbSet<SecurityEventLog> SecurityEventLogs => Set<SecurityEventLog>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
}