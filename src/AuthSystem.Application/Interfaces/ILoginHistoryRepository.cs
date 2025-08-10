using AuthSystem.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای دسترسی به داده‌های تاریخچه ورود
/// </summary>
public interface ILoginHistoryRepository
{
    Task<IReadOnlyCollection<LoginHistory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<PagedResult<LoginHistory>> GetPagedByUserIdAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task AddAsync(LoginHistory history, CancellationToken cancellationToken);
}