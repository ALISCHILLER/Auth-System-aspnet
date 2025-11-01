using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.Messaging;

internal sealed class NoOpSmsSender(ILogger<NoOpSmsSender> logger) : ISmsSender
{
    public Task SendAsync(string phoneNumber, string message, CancellationToken ct)
    {
        logger.LogInformation("SMS send requested to {PhoneNumber}: {Message}", phoneNumber, message);
        return Task.CompletedTask;
    }
}