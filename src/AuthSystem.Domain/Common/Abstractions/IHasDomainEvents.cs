namespace AuthSystem.Domain.Common.Abstractions;

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    IReadOnlyCollection<IDomainEvent> DequeueDomainEvents();
}