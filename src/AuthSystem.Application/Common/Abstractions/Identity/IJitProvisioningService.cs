using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Application.Common.Abstractions.Identity;

public interface IJitProvisioningService
{
    Task<User?> ProvisionAsync(string email, string? tenantId, CancellationToken cancellationToken);
}