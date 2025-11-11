using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Events;

/// <summary>
/// Carries contextual metadata about a domain event.
/// </summary>
public class DomainEventMetadata
{

    public Guid? UserId { get; set; }


    public string? IpAddress { get; set; }


    public string? UserAgent { get; set; }

    public string? SessionId { get; set; }


    public DateTime ProcessingStartedAt { get; } = DomainClock.Instance.UtcNow;

    public DateTime? ProcessingEndedAt { get; private set; }


    public bool IsSuccessful { get; private set; }

    public int AttemptCount { get; private set; }

    public void MarkAsCompleted(bool isSuccessful)
    {
        ProcessingEndedAt = DomainClock.Instance.UtcNow;
        IsSuccessful = isSuccessful;
    }

    public void IncrementAttemptCount() => AttemptCount++;


    public TimeSpan? GetProcessingDuration()
    {
        if (ProcessingEndedAt is null)
        {
            return null;
        }
        return ProcessingEndedAt.Value - ProcessingStartedAt;
    }
}