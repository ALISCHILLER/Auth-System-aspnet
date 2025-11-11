using AuthSystem.Domain.Common.Abstractions;
using MediatR;

namespace AuthSystem.Application.Common.Events;

public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent) : INotification
    where TDomainEvent : IDomainEvent;