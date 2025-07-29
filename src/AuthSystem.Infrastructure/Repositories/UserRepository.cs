using AuthSystem.Domain.Interfaces.Repositories;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using AuthSystem.Infrastructure.Persistence.Contexts;

namespace AuthSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// پیاده‌سازی IUserRepository
/// </summary>
public class UserRepository : GenericRepository<User>, IUserRepository
{
    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="context">DbContext</param>
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// دریافت کاربر بر اساس آدرس ایمیل
    /// </summary>
    /// <param name="email">آدرس ایمیل</param>
    /// <returns>کاربر یا null در صورت عدم وجود</returns>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.RefreshTokens)
            .Include(u => u.LoginHistories)
            .Include(u => u.UserDevices)
            .FirstOrDefaultAsync(u => u.EmailAddress.ToLower() == email.ToLower());
    }

    /// <summary>
    /// دریافت کاربر بر اساس نام کاربری
    /// </summary>
    /// <param name="username">نام کاربری</param>
    /// <returns>کاربر یا null در صورت عدم وجود</returns>
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.RefreshTokens)
            .Include(u => u.LoginHistories)
            .Include(u => u.UserDevices)
            .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
    }

    /// <summary>
    /// بررسی وجود ایمیل تکراری
    /// </summary>
    /// <param name="email">آدرس ایمیل</param>
    /// <param name="excludedUserId">شناسه کاربری که باید از بررسی مستثنی شود (برای ویرایش)</param>
    /// <returns>در صورت تکراری بودن true باز می‌گرداند</returns>
    public async Task<bool> EmailExistsAsync(string email, Guid? excludedUserId = null)
    {
        var query = _context.Users.Where(u => u.EmailAddress.ToLower() == email.ToLower());
        if (excludedUserId.HasValue)
        {
            query = query.Where(u => u.Id != excludedUserId.Value);
        }
        return await query.AnyAsync();
    }

    /// <summary>
    /// بررسی وجود نام کاربری تکراری
    /// </summary>
    /// <param name="username">نام کاربری</param>
    /// <param name="excludedUserId">شناسه کاربری که باید از بررسی مستثنی شود (برای ویرایش)</param>
    /// <returns>در صورت تکراری بودن true باز می‌گرداند</returns>
    public async Task<bool> UsernameExistsAsync(string username, Guid? excludedUserId = null)
    {
        var query = _context.Users.Where(u => u.UserName.ToLower() == username.ToLower());
        if (excludedUserId.HasValue)
        {
            query = query.Where(u => u.Id != excludedUserId.Value);
        }
        return await query.AnyAsync();
    }
}