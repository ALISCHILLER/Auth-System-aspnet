using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Infrastructure.Persistence.Sql;
using AuthSystem.Shared.Contracts.Security;
using AuthSystem.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AuthSystem.Infrastructure.SecurityEvents;

internal sealed class SecurityEventReader(ApplicationDbContext dbContext) : ISecurityEventReader
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<PagedResult<SecurityEventDto>> GetAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var query = dbContext.SecurityEventLogs.AsNoTracking();

        if (request.Filters.TryGetValue("tenantId", out var tenantId) && !string.IsNullOrWhiteSpace(tenantId))
        {
            query = query.Where(log => log.TenantId == tenantId);
        }

        if (request.Filters.TryGetValue("eventType", out var eventType) && !string.IsNullOrWhiteSpace(eventType))
        {
            query = query.Where(log => log.EventType == eventType);
        }

        var total = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var items = await query
            .OrderByDescending(log => log.OccurredAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(log => new SecurityEventDto
            {
                Id = log.Id,
                EventType = Enum.TryParse<SecurityEventType>(log.EventType, out var parsed) ? parsed : SecurityEventType.Login,
                UserId = log.UserId,
                UserName = log.UserName,
                TenantId = log.TenantId,
                OccurredAtUtc = log.OccurredAtUtc,
                IpAddress = log.IpAddress,
                UserAgent = log.UserAgent,
                Description = log.Description,
                Metadata = DeserializeMetadata(log.MetadataJson)
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PagedResult<SecurityEventDto>(items, request.PageNumber, request.PageSize, total);
    }

    private static IReadOnlyDictionary<string, string>? DeserializeMetadata(string? metadataJson)
    {
        if (string.IsNullOrWhiteSpace(metadataJson))
        {
            return null;
        }

        return JsonSerializer.Deserialize<Dictionary<string, string>>(metadataJson, SerializerOptions);
    }
}