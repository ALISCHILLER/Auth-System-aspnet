using AuthSystem.Domain.Common.Abstractions;

namespace AuthSystem.Domain.Common.Events;


public interface IAsyncDomainEvent : IDomainEvent
{

    Task HandleAsync();
}