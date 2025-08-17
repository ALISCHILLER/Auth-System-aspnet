using AuthSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common;

/// <summary>
/// کلاس پایه برای Aggregate Roots در معماری DDD
/// این کلاس علاوه بر قابلیت‌های BaseEntity، مدیریت رویدادها و نسخه‌بندی را نیز انجام می‌دهد
/// </summary>
public abstract class AggregateRoot<TId> : BaseEntity<TId>, IAuditableEntity
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// لیست رویدادهای دامنه که در این تراکنش رخ داده‌اند
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// نسخه موجودیت برای Optimistic Concurrency Control
    /// هر بار که موجودیت تغییر می‌کند، این عدد افزایش می‌یابد
    /// </summary>
    public int Version { get; protected set; }

    /// <summary>
    /// سازنده پیش‌فرض
    /// </summary>
    protected AggregateRoot() : base() { }

    /// <summary>
    /// سازنده با شناسه
    /// </summary>
    protected AggregateRoot(TId id) : base(id) { }

    /// <summary>
    /// افزودن رویداد دامنه
    /// این رویدادها پس از ذخیره موجودیت منتشر می‌شوند
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent == null)
            throw new ArgumentNullException(nameof(domainEvent));

        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// افزودن رویداد دامنه به صورت شرطی
    /// </summary>
    protected void AddDomainEventIf(bool condition, Func<IDomainEvent> eventFactory)
    {
        if (condition && eventFactory != null)
        {
            AddDomainEvent(eventFactory());
        }
    }

    /// <summary>
    /// حذف رویداد دامنه خاص
    /// </summary>
    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// پاک کردن تمام رویدادهای دامنه
    /// معمولاً پس از publish شدن رویدادها فراخوانی می‌شود
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// بررسی وجود رویداد خاص
    /// </summary>
    public bool HasDomainEvent<T>() where T : IDomainEvent
    {
        return _domainEvents.Any(e => e is T);
    }

    /// <summary>
    /// دریافت رویدادهای خاص
    /// </summary>
    public IEnumerable<T> GetDomainEvents<T>() where T : IDomainEvent
    {
        return _domainEvents.OfType<T>();
    }

    /// <summary>
    /// افزایش نسخه برای Concurrency Control
    /// </summary>
    protected void IncrementVersion()
    {
        Version++;
    }

    /// <summary>
    /// به‌روزرسانی موجودیت و افزایش نسخه
    /// </summary>
    protected override void MarkAsUpdated()
    {
        base.MarkAsUpdated();
        IncrementVersion();
    }

    /// <summary>
    /// بررسی قانون کسب‌وکار به صورت async
    /// </summary>
    protected static async Task CheckRuleAsync(IAsyncBusinessRule rule)
    {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule));

        if (await rule.IsBrokenAsync())
        {
            throw new BusinessRuleValidationException(rule.Message, rule.ErrorCode);
        }
    }

    /// <summary>
    /// بررسی چندین قانون کسب‌وکار به صورت موازی
    /// </summary>
    protected static async Task CheckRulesAsync(params IAsyncBusinessRule[] rules)
    {
        if (rules == null || rules.Length == 0)
            return;

        var brokenRules = new List<(string Message, string ErrorCode)>();

        // بررسی موازی تمام قوانین
        var tasks = rules.Select(async rule =>
        {
            if (await rule.IsBrokenAsync())
                return (rule.Message, rule.ErrorCode);
            return (null, null);
        });

        var results = await Task.WhenAll(tasks);

        brokenRules.AddRange(results.Where(r => r.Item1 != null)!);

        if (brokenRules.Any())
        {
            throw new AggregateBusinessRuleValidationException(brokenRules);
        }
    }

    /// <summary>
    /// اعمال رویداد بر روی Aggregate (برای Event Sourcing)
    /// </summary>
    protected virtual void Apply(IDomainEvent @event)
    {
        // این متد در صورت استفاده از Event Sourcing باید override شود
        throw new NotImplementedException(
            "برای استفاده از Event Sourcing، متد Apply باید پیاده‌سازی شود");
    }

    /// <summary>
    /// بازسازی Aggregate از رویدادها (Event Sourcing)
    /// </summary>
    public virtual void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        foreach (var @event in history)
        {
            Apply(@event);
            Version++;
        }
    }
}
