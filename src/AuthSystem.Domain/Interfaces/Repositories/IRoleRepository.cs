using AuthSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Interfaces.Repositories;

/// <summary>
/// رابط Repository برای موجودیت Role
/// این رابط عملیات خاص نقش‌ها را تعریف می‌کند
/// </summary>
public interface IRoleRepository : IGenericRepository<Role>
{
    /// <summary>
    /// دریافت نقش بر اساس نام
    /// </summary>
    /// <param name="name">نام نقش</param>
    /// <returns>نقش یا null در صورت عدم وجود</returns>
    Task<Role?> GetByNameAsync(string name);

    /// <summary>
    /// بررسی وجود نام تکراری برای نقش
    /// </summary>
    /// <param name="name">نام نقش</param>
    /// <param name="excludedRoleId">شناسه نقشی که باید از بررسی مستثنی شود (برای ویرایش)</param>
    /// <returns>در صورت تکراری بودن true باز می‌گرداند</returns>
    Task<bool> NameExistsAsync(string name, Guid? excludedRoleId = null);
}