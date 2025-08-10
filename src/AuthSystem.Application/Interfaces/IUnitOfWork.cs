using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای مدیریت تراکنش‌های دیتابیس
/// </summary>
public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IPermissionRepository Permissions { get; }
    IUserDeviceRepository UserDevices { get; }
    ILoginHistoryRepository LoginHistories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}