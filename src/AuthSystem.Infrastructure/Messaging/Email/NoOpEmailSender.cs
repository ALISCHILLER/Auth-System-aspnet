using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Messaging;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.Messaging.Email;

internal sealed class NoOpEmailSender(ILogger<NoOpEmailSender> logger) : IEmailSender
{
    public Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct)
    {
        logger.LogDebug("Skipping email send to {Recipient}. Subject: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}