using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Models;

namespace AuthSystem.Application.Common.Abstractions.Monitoring;

/// <summary>
/// Publishes security-related events to downstream systems and durable stores.
/// </summary>
public interface ISecurityEventPublisher
{
    Task PublishAsync(SecurityEventContext context, CancellationToken cancellationToken);
}