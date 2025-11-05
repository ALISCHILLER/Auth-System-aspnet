using System;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.DomainEvents;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Infrastructure.Persistence.Sql.Repositories;

internal sealed class UserRepository(
    ApplicationDbContext dbContext,
    IDomainEventCollector domainEventCollector) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken).ConfigureAwait(false);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await dbContext.Users
            .FirstOrDefaultAsync(user => user.Email != null && user.Email.Value == email, cancellationToken)
            .ConfigureAwait(false);

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        domainEventCollector.CollectFrom(user);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        dbContext.Users.Update(user);
        domainEventCollector.CollectFrom(user);
        return Task.CompletedTask;
    }
}