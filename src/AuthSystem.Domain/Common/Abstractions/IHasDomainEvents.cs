using System.Collections.Generic;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Common.Abstractions;

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    IReadOnlyCollection<IDomainEvent> DequeueDomainEvents();
}