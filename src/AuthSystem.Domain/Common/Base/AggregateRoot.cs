using AuthSystem.Domain.Common.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AuthSystem.Domain.Common.Abstractions;
using AuthSystem.Domain.Common.Exceptions;


namespace AuthSystem.Domain.Common.Base;

public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvent
    where TId : notnull
{

    private static readonly ConcurrentDictionary<(Type Aggregate, Type Event), MethodInfo?> HandlerCache = new();
    private readonly List<IDomainEvent> _domainEvents = new();


    protected AggregateRoot()
    {

    }


    protected AggregateRoot(TId id) : base(id)
    {

    }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    public int Version { get; protected set; }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        _domainEvents.Add(domainEvent);
    }


    public void RemoveDomainEvent(IDomainEvent? domainEvent)
    {
        if (domainEvent is null)
        {
            return;
        }

        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
    public bool HasDomainEvents() => _domainEvents.Count > 0;

    protected void IncrementVersion() => Version++;

    protected virtual void Apply(IDomainEvent domainEvent)
    {
        var handler = ResolveHandler(domainEvent)
            ?? throw new InvalidOperationException(
                $"No handler for event type {domainEvent.GetType().Name} was found in aggregate {GetType().Name}. Define a method named On({domainEvent.GetType().Name}).");

        handler.Invoke(this, new object[] { domainEvent });
    }

    protected virtual async Task ApplyAsync(IDomainEvent domainEvent)
    {
        Apply(domainEvent);
        if (domainEvent is IAsyncDomainEvent asyncEvent)
        {
            await asyncEvent.HandleAsync().ConfigureAwait(false);
        }
    }
    protected void ApplyRaise(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        Apply(domainEvent);
        AddDomainEvent(domainEvent);
        IncrementVersion();
    }

    protected async Task ApplyRaiseAsync(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        await ApplyAsync(domainEvent).ConfigureAwait(false);
        AddDomainEvent(domainEvent);
        IncrementVersion();
    }

    public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
    {
        var events = _domainEvents.ToArray();
        _domainEvents.Clear();
        return events;
    }

    public virtual void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        ArgumentNullException.ThrowIfNull(history);

        foreach (var @event in history)
        {
            Apply(@event);
            Version++;
        }

        ClearDomainEvents();
    }

    protected static void CheckRule(IBusinessRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        if (rule.IsBroken())
        {
            throw BusinessRuleValidationException.ForBrokenRule(rule);
        }
    }

    protected static async Task CheckRuleAsync(IAsyncBusinessRule rule)
     {
       ArgumentNullException.ThrowIfNull(rule);

        if (await rule.IsBrokenAsync().ConfigureAwait(false))
          {
            throw await BusinessRuleValidationException.ForBrokenRuleAsync(rule).ConfigureAwait(false);
          }
     }

    protected static async Task CheckRulesAsync(IEnumerable<IAsyncBusinessRule> rules)
    {
        if (rules is null)
        {
            return;
        }

        var brokenRules = new List<IAsyncBusinessRule>();
        foreach (var rule in rules)
        {
            if (await rule.IsBrokenAsync().ConfigureAwait(false))
            {
                brokenRules.Add(rule);
            }
        }

        if (brokenRules.Count > 0)
        {
            throw await AggregateBusinessRuleValidationException
                .ForMultipleBrokenRulesAsync(brokenRules)
                .ConfigureAwait(false);
        }
    }

    private MethodInfo? ResolveHandler(IDomainEvent domainEvent)
    {
        var aggregateType = GetType();
        var eventType = domainEvent.GetType();
        var cacheKey = (aggregateType, eventType);

        if (HandlerCache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }
        var handler = FindHandler(aggregateType, eventType);
        HandlerCache[cacheKey] = handler;
        return handler;
    }

    private static MethodInfo? FindHandler(Type aggregateType, Type eventType)
    {
        MethodInfo? method = null;
        var currentEventType = eventType;

        while (currentEventType is not null && currentEventType != typeof(object) && method is null)
        {
            method = aggregateType.GetMethod(
                          "On",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                types: new[] { currentEventType },
                modifiers: null);

        currentEventType = currentEventType.BaseType;
        }

        return method;
    }
}