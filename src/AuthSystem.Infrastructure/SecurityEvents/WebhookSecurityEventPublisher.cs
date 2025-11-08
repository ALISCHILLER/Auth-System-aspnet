using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Application.Common.Models;
using AuthSystem.Infrastructure.Auditing;
using AuthSystem.Infrastructure.Options;
using AuthSystem.Infrastructure.Persistence.Sql;
using AuthSystem.Shared.Contracts.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthSystem.Infrastructure.SecurityEvents;

public sealed class WebhookSecurityEventPublisher(
    ApplicationDbContext dbContext,
    IHttpClientFactory httpClientFactory,
    IOptions<SecurityWebhookOptions> options,
    ILogger<WebhookSecurityEventPublisher> logger)
    : ISecurityEventPublisher
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task PublishAsync(SecurityEventContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        var metadataJson = context.Metadata is null ? null : JsonSerializer.Serialize(context.Metadata, SerializerOptions);

        var logEntry = new SecurityEventLog
        {
            Id = Guid.NewGuid(),
            EventType = context.EventType.ToString(),
            UserId = context.UserId,
            UserName = context.UserName,
            TenantId = context.TenantId,
            OccurredAtUtc = DateTime.UtcNow,
            IpAddress = context.IpAddress,
            UserAgent = context.UserAgent,
            Description = context.Description,
            MetadataJson = metadataJson
        };

        await dbContext.SecurityEventLogs.AddAsync(logEntry, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var webhookOptions = options.Value;
        if (!webhookOptions.Enabled || webhookOptions.Subscriptions.Count == 0)
        {
            return;
        }

        var jsonPayload = JsonSerializer.Serialize(new
        {
            logEntry.Id,
            EventType = context.EventType,
            context.UserId,
            context.UserName,
            context.TenantId,
            context.IpAddress,
            context.UserAgent,
            context.Description,
            context.Metadata,
            logEntry.OccurredAtUtc
        }, SerializerOptions);

        foreach (var subscription in webhookOptions.Subscriptions.Where(ShouldSendToSubscription))
        {
            try
            {
                var client = httpClientFactory.CreateClient("security-webhooks");
                using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                foreach (var header in subscription.Headers)
                {
                    content.Headers.Remove(header.Key);
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                if (!string.IsNullOrWhiteSpace(subscription.Secret))
                {
                    var signature = ComputeSignature(subscription.Secret!, jsonPayload);
                    content.Headers.Remove("X-Signature");
                    content.Headers.TryAddWithoutValidation("X-Signature", signature);
                }

                var response = await client.PostAsync(subscription.Url, content, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning(
                        "Security webhook '{Subscription}' responded with {StatusCode}",
                        subscription.Name,
                        response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to publish security webhook '{Subscription}'", subscription.Name);
            }
        }

        bool ShouldSendToSubscription(SecurityWebhookSubscription subscription)
            => subscription.EventTypes.Count == 0
                || subscription.EventTypes.Any(type => string.Equals(type, logEntry.EventType, StringComparison.OrdinalIgnoreCase));
    }

    private static string ComputeSignature(string secret, string payload)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash);
    }
}