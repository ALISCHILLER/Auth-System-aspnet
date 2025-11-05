using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Common.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}