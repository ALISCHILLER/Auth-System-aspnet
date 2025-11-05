using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.DomainEvents;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Domain.Entities.Authorization.Role;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Infrastructure.Persistence.Sql.Repositories;

internal sealed class RoleRepository(
    ApplicationDbContext dbContext,
    IDomainEventCollector domainEventCollector) : IRoleRepository
{
    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.Roles.FirstOrDefaultAsync(role => role.Id == id, cancellationToken).ConfigureAwait(false);

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => await dbContext.Roles.FirstOrDefaultAsync(role => role.Name == name, cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<Role>> GetAllAsync(CancellationToken cancellationToken)
        => await dbContext.Roles.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);

    public async Task AddAsync(Role role, CancellationToken cancellationToken)
    {
        await dbContext.Roles.AddAsync(role, cancellationToken).ConfigureAwait(false);
        domainEventCollector.CollectFrom(role);
    }

    public Task UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        dbContext.Roles.Update(role);
        domainEventCollector.CollectFrom(role);
        return Task.CompletedTask;
    }
}