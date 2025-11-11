using AuthSystem.Domain.Common.Abstractions;

namespace AuthSystem.Domain.Common.Events;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
    }

    public Guid EventId { get; }

    public DateTime OccurredOnUtc { get; }

    public DateTime OccurredOn => OccurredOnUtc;

    public bool IsPublished { get; private set; }

    public void MarkAsPublished() => IsPublished = true;
}