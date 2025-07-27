using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Repositories;
using AuthSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Infrastructure.Repositories;

/// <summary>
/// پیاده‌سازی Repository کاربران
/// </summary>
public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="context">DbContext</param>
    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// پیدا کردن کاربر بر اساس نام کاربری
    /// </summary>
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles) // بارگذاری نقش‌ها
                .ThenInclude(ur => ur.Role)
            .Include(u => u.RefreshTokens) // بارگذاری توکن‌ها
            .FirstOrDefaultAsync(u => u.UserName == username);
    }

    /// <summary>
    /// پیدا کردن کاربر بر اساس ایمیل
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <summary>
    /// بررسی وجود نام کاربری
    /// </summary>
    public async Task<bool> IsUsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username);
    }

    /// <summary>
    /// بررسی وجود ایمیل
    /// </summary>
    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}