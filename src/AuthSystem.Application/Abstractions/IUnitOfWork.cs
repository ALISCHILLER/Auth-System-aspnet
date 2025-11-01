using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Abstractions;

public interface IUnitOfWork
{
    Task BeginAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
    Task RollbackAsync(CancellationToken ct);
}