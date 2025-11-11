using AuthSystem.Application.Common.Abstractions.Messaging;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.Messaging.Sms;

internal sealed class NoOpSmsSender(ILogger<NoOpSmsSender> logger) : ISmsSender
{
    public Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken)
    {
        logger.LogDebug("Skipping SMS send to {PhoneNumber}. Message length: {Length}", phoneNumber, message.Length);
        return Task.CompletedTask;
    }
}