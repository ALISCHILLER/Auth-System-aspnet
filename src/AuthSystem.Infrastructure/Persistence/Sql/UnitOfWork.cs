using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Persistence;

namespace AuthSystem.Infrastructure.Persistence.Sql;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}