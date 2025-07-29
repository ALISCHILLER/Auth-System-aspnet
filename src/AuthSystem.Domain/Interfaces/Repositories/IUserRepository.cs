using AuthSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Interfaces.Repositories;

/// <summary>
/// رابط Repository برای موجودیت User
/// این رابط عملیات خاص کاربران را تعریف می‌کند
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// دریافت کاربر بر اساس آدرس ایمیل
    /// </summary>
    /// <param name="email">آدرس ایمیل</param>
    /// <returns>کاربر یا null در صورت عدم وجود</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// دریافت کاربر بر اساس نام کاربری
    /// </summary>
    /// <param name="username">نام کاربری</param>
    /// <returns>کاربر یا null در صورت عدم وجود</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// بررسی وجود ایمیل تکراری
    /// </summary>
    /// <param name="email">آدرس ایمیل</param>
    /// <param name="excludedUserId">شناسه کاربری که باید از بررسی مستثنی شود (برای ویرایش)</param>
    /// <returns>در صورت تکراری بودن true باز می‌گرداند</returns>
    Task<bool> EmailExistsAsync(string email, Guid? excludedUserId = null);

    /// <summary>
    /// بررسی وجود نام کاربری تکراری
    /// </summary>
    /// <param name="username">نام کاربری</param>
    /// <param name="excludedUserId">شناسه کاربری که باید از بررسی مستثنی شود (برای ویرایش)</param>
    /// <returns>در صورت تکراری بودن true باز می‌گرداند</returns>
    Task<bool> UsernameExistsAsync(string username, Guid? excludedUserId = null);
}