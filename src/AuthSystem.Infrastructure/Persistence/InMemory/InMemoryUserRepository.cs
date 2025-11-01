using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Infrastructure.Persistence.InMemory;

internal sealed class InMemoryUserRepository(
    InMemoryDatabase database,
    IDomainEventCollector eventCollector) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        database.Users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }

    public Task<User?> GetByEmailAsync(Email email, CancellationToken ct)
    {
        var user = database.Users.Values.FirstOrDefault(u => u.Email is not null && u.Email.Equals(email));
        return Task.FromResult(user);
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        var user = database.Users.Values.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task AddAsync(User aggregate, CancellationToken ct)
    {
        database.Users[aggregate.Id] = aggregate;
        eventCollector.CollectFromAggregate(aggregate);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User aggregate, CancellationToken ct)
    {
        database.Users[aggregate.Id] = aggregate;
        eventCollector.CollectFromAggregate(aggregate);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User aggregate, CancellationToken ct)
    {
        database.Users.TryRemove(aggregate.Id, out _);
        eventCollector.CollectFromAggregate(aggregate);
        return Task.CompletedTask;
    }
}