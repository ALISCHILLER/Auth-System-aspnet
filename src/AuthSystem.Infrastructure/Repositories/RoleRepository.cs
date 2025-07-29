using AuthSystem.Domain.Interfaces.Repositories;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// پیاده‌سازی IRoleRepository
/// </summary>
public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="context">DbContext</param>
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// دریافت نقش بر اساس نام
    /// </summary>
    /// <param name="name">نام نقش</param>
    /// <returns>نقش یا null در صورت عدم وجود</returns>
    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
    }

    /// <summary>
    /// بررسی وجود نام تکراری برای نقش
    /// </summary>
    /// <param name="name">نام نقش</param>
    /// <param name="excludedRoleId">شناسه نقشی که باید از بررسی مستثنی شود (برای ویرایش)</param>
    /// <returns>در صورت تکراری بودن true باز می‌گرداند</returns>
    public async Task<bool> NameExistsAsync(string name, Guid? excludedRoleId = null)
    {
        var query = _context.Roles.Where(r => r.Name.ToLower() == name.ToLower());
        if (excludedRoleId.HasValue)
        {
            query = query.Where(r => r.Id != excludedRoleId.Value);
        }
        return await query.AnyAsync();
    }
}