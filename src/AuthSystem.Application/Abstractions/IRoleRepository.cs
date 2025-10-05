using AuthSystem.Domain.Entities.Authorization.Role;

namespace AuthSystem.Application.Abstractions;

public interface IRoleRepository : IAggregateRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken ct);
}