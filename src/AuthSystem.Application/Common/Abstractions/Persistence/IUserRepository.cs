using System;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Application.Common.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}