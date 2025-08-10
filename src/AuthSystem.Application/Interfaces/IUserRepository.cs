using AuthSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای دسترسی به داده‌های کاربر
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// دریافت کاربر بر اساس شناسه
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// دریافت کاربر بر اساس ایمیل
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// دریافت کاربر بر اساس نام کاربری
    /// </summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// بررسی وجود کاربر با ایمیل مشخص
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// بررسی وجود کاربر با نام کاربری مشخص
    /// </summary>
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// دریافت نقش‌های کاربر
    /// </summary>
    Task<IReadOnlyCollection<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// دریافت مجوزهای کاربر
    /// </summary>
    Task<IReadOnlyCollection<Permission>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// دریافت لیست کاربران با صفحه‌بندی
    /// </summary>
    Task<PagedResult<User>> GetPagedUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// افزودن کاربر جدید
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// به‌روزرسانی کاربر
    /// </summary>
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}

/// <summary>
/// مدل نتیجه صفحه‌بندی شده
/// </summary>
public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}