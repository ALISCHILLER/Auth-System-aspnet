using AuthSystem.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای دسترسی به داده‌های دستگاه‌های کاربر
/// </summary>
public interface IUserDeviceRepository
{
    Task<UserDevice?> GetByDeviceIdAsync(string deviceId, Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<UserDevice>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task AddAsync(UserDevice device, CancellationToken cancellationToken);
    Task UpdateAsync(UserDevice device, CancellationToken cancellationToken);
}