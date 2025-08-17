using System;
using System.Threading.Tasks;
using AuthSystem.Domain.Aggregates.UserAggregate;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Aggregates.UserAggregate;

/// <summary>
/// رابط دسترسی به داده‌های کاربر
/// این رابط تمام عملیات مورد نیاز برای مدیریت کاربران را تعریف می‌کند
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// دریافت کاربر بر اساس شناسه
    /// </summary>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// دریافت کاربر بر اساس نام کاربری
    /// </summary>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// دریافت کاربر بر اساس آدرس ایمیل
    /// </summary>
    Task<User?> GetByEmailAsync(Email email);

    /// <summary>
    /// افزودن کاربر جدید
    /// </summary>
    Task AddAsync(User user);

    /// <summary>
    /// به‌روزرسانی کاربر
    /// </summary>
    Task UpdateAsync(User user);

    /// <summary>
    /// حذف کاربر
    /// </summary>
    Task DeleteAsync(User user);
}