using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای دسترسی به داده‌های مجوز
/// </summary>
public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Permission>> GetByTypeAsync(PermissionType type, CancellationToken cancellationToken);
    Task AddAsync(Permission permission, CancellationToken cancellationToken);
    Task UpdateAsync(Permission permission, CancellationToken cancellationToken);
}