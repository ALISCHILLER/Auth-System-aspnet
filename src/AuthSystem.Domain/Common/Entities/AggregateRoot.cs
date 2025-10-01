using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Common.Rules;
using System.Threading.Tasks;
namespace AuthSystem.Domain.Common.Entities;

/// <summary>
/// Base type for aggregate roots providing domain event handling and rule checking helpers.
/// </summary>
public abstract class AggregateRoot<TId> : BaseEntity<TId>
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
        if (domainEvent is null)
        {
            throw new ArgumentNullException(nameof(domainEvent));
        }

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
        if (domainEvent is null)
        {
            throw new ArgumentNullException(nameof(domainEvent));
        }

        Apply(domainEvent);
        AddDomainEvent(domainEvent);
        IncrementVersion();
    }

    protected async Task ApplyRaiseAsync(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
        {
            throw new ArgumentNullException(nameof(domainEvent));
        }
        await ApplyAsync(domainEvent).ConfigureAwait(false);
        AddDomainEvent(domainEvent);
        IncrementVersion();
    }

    public virtual void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        if (history is null)
        {
            throw new ArgumentNullException(nameof(history));
        }

        foreach (var @event in history)
        {
            Apply(@event);
            Version++;
        }

        ClearDomainEvents();
    }

    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule is null)
        {
            throw new ArgumentNullException(nameof(rule));
        }

        if (rule.IsBroken())
        {
            throw BusinessRuleValidationException.ForBrokenRule(rule);
        }
    }

    protected static async Task CheckRuleAsync(IAsyncBusinessRule rule)
    {
        if (rule is null)
        {
            throw new ArgumentNullException(nameof(rule));
        }

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