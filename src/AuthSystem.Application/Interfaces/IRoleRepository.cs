using AuthSystem.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای دسترسی به داده‌های نقش
/// </summary>
public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<PagedResult<Role>> GetPagedRolesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task AddAsync(Role role, CancellationToken cancellationToken);
    Task UpdateAsync(Role role, CancellationToken cancellationToken);
}