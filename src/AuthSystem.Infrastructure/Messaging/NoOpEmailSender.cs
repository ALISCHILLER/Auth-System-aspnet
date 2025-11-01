using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.Messaging;

internal sealed class NoOpEmailSender(ILogger<NoOpEmailSender> logger) : IEmailSender
{
    public Task SendAsync(string to, string subject, string body, CancellationToken ct)
    {
        logger.LogInformation("Email send requested to {Recipient} with subject '{Subject}'", to, subject);
        return Task.CompletedTask;
    }
}