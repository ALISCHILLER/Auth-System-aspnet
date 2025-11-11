using AuthSystem.Application.Common.Abstractions.Persistence;

namespace AuthSystem.Infrastructure.Persistence.Sql;

internal sealed class SqlApplicationDbContext(ApplicationDbContext dbContext) : IApplicationDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}