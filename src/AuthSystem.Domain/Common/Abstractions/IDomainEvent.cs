namespace AuthSystem.Domain.Common.Abstractions;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOnUtc { get; }
    DateTime OccurredOn { get; }
    bool IsPublished { get; }
    void MarkAsPublished();
}