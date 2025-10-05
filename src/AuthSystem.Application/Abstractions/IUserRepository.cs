using AuthSystem.Domain.ValueObjects;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Application.Abstractions;

public interface IUserRepository : IAggregateRepository<User>
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken ct);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
}