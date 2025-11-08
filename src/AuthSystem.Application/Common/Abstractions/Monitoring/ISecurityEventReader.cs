using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Shared.Contracts;
using AuthSystem.Shared.Contracts.Security;

namespace AuthSystem.Application.Common.Abstractions.Monitoring;

/// <summary>
/// Provides read access to persisted security audit events.
/// </summary>
public interface ISecurityEventReader
{
    Task<PagedResult<SecurityEventDto>> GetAsync(PagedRequest request, CancellationToken cancellationToken);
}