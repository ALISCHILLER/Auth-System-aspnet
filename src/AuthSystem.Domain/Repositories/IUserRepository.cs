using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities;
namespace AuthSystem.Domain.Repositories;

/// <summary>
/// Interface برای Repository کاربران
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// پیدا کردن کاربر بر اساس نام کاربری
    /// </summary>
    /// <param name="username">نام کاربری</param>
    /// <returns>کاربر یا null</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// پیدا کردن کاربر بر اساس ایمیل
    /// </summary>
    /// <param name="email">ایمیل</param>
    /// <returns>کاربر یا null</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// بررسی وجود نام کاربری
    /// </summary>
    /// <param name="username">نام کاربری</param>
    /// <returns>True اگر وجود داشته باشد</returns>
    Task<bool> IsUsernameExistsAsync(string username);

    /// <summary>
    /// بررسی وجود ایمیل
    /// </summary>
    /// <param name="email">ایمیل</param>
    /// <returns>True اگر وجود داشته باشد</returns>
    Task<bool> IsEmailExistsAsync(string email);
}