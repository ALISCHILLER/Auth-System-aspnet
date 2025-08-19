using Microsoft.EntityFrameworkCore;
using AuthSystem.Domain.Entities.Authorization.Role;

namespace AuthSystem.Application.Common.Abstractions.Persistence;

/// <summary>
/// کانتکست دیتابیس کاربردی
/// فقط Entityها را اکسپوز می‌کند و SaveChanges را فراهم می‌سازد
/// </summary>
public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Permission> Permissions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
