using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Common.Abstractions.Persistence;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}