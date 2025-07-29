using System;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Interfaces.Repositories;

/// <summary>
/// رابط Unit of Work
/// این رابط به کنترل تراکنش‌ها و ذخیره تغییرات کمک می‌کند
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// دسترسی به Repository کاربران
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// دسترسی به Repository نقش‌ها
    /// </summary>
    IRoleRepository Roles { get; }

    // در آینده می‌توانید سایر Repositoryها را اضافه کنید
    // IPermissionRepository Permissions { get; }

    /// <summary>
    /// ذخیره تغییرات در دیتابیس
    /// </summary>
    /// <returns>تعداد رکوردهای تغییر کرده</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// شروع یک تراکنش جدید
    /// </summary>
    /// <returns>تراکنش</returns>
    Task<IDisposable> BeginTransactionAsync();
}