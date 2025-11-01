using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Entities.Authorization.Role;

namespace AuthSystem.Infrastructure.Persistence.InMemory;

internal sealed class InMemoryRoleRepository(
    InMemoryDatabase database,
    IDomainEventCollector eventCollector) : IRoleRepository
{
    public Task<Role?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        database.Roles.TryGetValue(id, out var role);
        return Task.FromResult(role);
    }

    public Task<Role?> GetByNameAsync(string name, CancellationToken ct)
    {
        var role = database.Roles.Values.FirstOrDefault(r => string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(role);
    }

    public Task AddAsync(Role aggregate, CancellationToken ct)
    {
        database.Roles[aggregate.Id] = aggregate;
        eventCollector.CollectFromAggregate(aggregate);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Role aggregate, CancellationToken ct)
    {
        database.Roles[aggregate.Id] = aggregate;
        eventCollector.CollectFromAggregate(aggregate);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Role aggregate, CancellationToken ct)
    {
        database.Roles.TryRemove(aggregate.Id, out _);
        eventCollector.CollectFromAggregate(aggregate);
        return Task.CompletedTask;
    }
}