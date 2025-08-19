using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Common.Entities;

/// <summary>
/// ریشهٔ تجمع (Aggregate Root)
/// - نگهداری صف رویدادهای دامنه
/// - نقطهٔ ورود تغییرات سازگار در یک Aggregate
/// - رفع مشکل جستجوی متد On با استفاده از Reflection و کش
/// </summary>
public abstract class AggregateRoot<TId> : BaseEntity<TId>
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private static readonly Dictionary<(Type AggregateType, Type EventType), MethodInfo?> _onMethodCache = new();

    /// <summary>
    /// رویدادهای معلق دامنه (فقط خواندنی)
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// نسخه برای کنترل Concurrency
    /// </summary>
    public int Version { get; protected set; } = 0;

    /// <summary>
    /// سازنده پیش‌فرض برای EF Core
    /// </summary>
    protected AggregateRoot()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی با شناسه
    /// </summary>
    protected AggregateRoot(TId id) : base(id)
    {
        // تنظیمات اولیه
    }

    /// <summary>
    /// افزودن رویداد دامنه به صف انتشار
    /// </summary>
    protected void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    /// <summary>
    /// افزودن شرطی رویداد دامنه
    /// </summary>
    protected void AddDomainEventIf(bool condition, Func<IDomainEvent> eventFactory)
    {
        if (condition)
            AddDomainEvent(eventFactory());
    }

    /// <summary>
    /// حذف یک رویداد دامنه
    /// </summary>
    public void RemoveDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Remove(@event);
    }

    /// <summary>
    /// پاک‌سازی همهٔ رویدادهای معلق
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// آیا رویداد خاصی وجود دارد؟
    /// </summary>
    public bool HasDomainEvents() => _domainEvents.Any();

    /// <summary>
    /// افزایش نسخه برای Concurrency Control
    /// </summary>
    protected void IncrementVersion() => Version++;

    /// <summary>
    /// علامت‌گذاری به‌عنوان به‌روزرسانی‌شده + افزایش نسخه
    /// </summary>
    protected override void MarkAsUpdated(Guid? updatedBy = null)
    {
        base.MarkAsUpdated(updatedBy);
        IncrementVersion();
    }

    /// <summary>
    /// بررسی قانون همزمان (Sync)
    /// </summary>
    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule is null)
            throw new ArgumentNullException(nameof(rule));

        if (rule.IsBroken())
            throw BusinessRuleValidationException.ForBrokenRule(rule);
    }

    /// <summary>
    /// بررسی قانون ناهمزمان (Async)
    /// </summary>
    protected static async Task CheckRuleAsync(IAsyncBusinessRule rule)
    {
        if (rule is null)
            throw new ArgumentNullException(nameof(rule));

        if (await rule.IsBrokenAsync().ConfigureAwait(false))
            throw BusinessRuleValidationException.ForBrokenRule(rule);
    }

    /// <summary>
    /// بررسی چندین قانون Async به صورت موازی
    /// </summary>
    protected static async Task CheckRulesAsync(IEnumerable<IAsyncBusinessRule> rules)
    {
        if (rules == null || !rules.Any())
            return;

        var brokenRules = await Task.WhenAll(rules.Select(async r =>
            (Rule: r, Broken: await r.IsBrokenAsync().ConfigureAwait(false))));

        var broken = brokenRules
            .Where(x => x.Broken)
            .Select(x => x.Rule)
            .ToList();

        if (broken.Any())
        {
            // راه‌حل اصلی: استفاده از ForMultipleBrokenRulesAsync به جای ForMultipleBrokenRules
            var exception = await AggregateBusinessRuleValidationException.ForMultipleBrokenRulesAsync(broken);
            throw exception;
        }
    }

    /// <summary>
    /// اعمال یک رویداد روی Aggregate
    /// - از Reflection برای یافتن متد On استفاده می‌کند
    /// - از کش برای جلوگیری از جستجوی مکرر استفاده می‌کند
    /// - هم متد‌های public و هم private را پیدا می‌کند
    /// - والدین رویداد را نیز بررسی می‌کند
    /// </summary>
    protected virtual void Apply(IDomainEvent @event)
    {
        var key = (GetType(), @event.GetType());

        // بررسی کش برای متد On
        if (!_onMethodCache.TryGetValue(key, out var method))
        {
            // یافتن متد On با استفاده از Reflection
            method = FindOnMethod(@event);

            // ذخیره در کش (حتی اگر null باشد)
            _onMethodCache[key] = method;
        }

        if (method == null)
        {
            throw new InvalidOperationException(
                $"هیچ متدی برای پردازش رویداد {@event.GetType().Name} یافت نشد. " +
                $"لطفاً متد 'On({@event.GetType().Name})' را در کلاس {GetType().Name} تعریف کنید.");
        }

        // فراخوانی متد On
        method.Invoke(this, new object[] { @event });
    }

    /// <summary>
    /// اعمال ناهمزمان یک رویداد روی Aggregate
    /// </summary>
    protected virtual async Task ApplyAsync(IDomainEvent @event)
    {
        Apply(@event);

        if (@event is IAsyncDomainEvent asyncEvent)
            await asyncEvent.HandleAsync();
    }

    /// <summary>
    /// اعمال و افزودن رویداد به صف (برای انتشار بعدی)
    /// - ابتدا حالت را تغییر می‌دهد
    /// - سپس رویداد را برای انتشار اضافه می‌کند
    /// - نسخه را افزایش می‌دهد
    /// </summary>
    protected void ApplyRaise(IDomainEvent @event)
    {
        Apply(@event);
        AddDomainEvent(@event);
        IncrementVersion();
    }

    /// <summary>
    /// اعمال و افزودن رویداد به صف به صورت ناهمزمان
    /// </summary>
    protected async Task ApplyRaiseAsync(IDomainEvent @event)
    {
        await ApplyAsync(@event);
        AddDomainEvent(@event);
        IncrementVersion();
    }

    /// <summary>
    /// بازسازی Aggregate از تاریخچه رویدادها
    /// - فقط اعمال رویدادها بدون افزودن به صف انتشار
    /// - برای بازیابی از دیتابیس استفاده می‌شود
    /// </summary>
    public virtual void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        if (history == null) throw new ArgumentNullException(nameof(history));

        foreach (var e in history)
        {
            Apply(e);
            Version++;
        }
    }

    /// <summary>
    /// یافتن متد On با استفاده از Reflection
    /// - هم متد‌های public و هم private را پیدا می‌کند
    /// - والدین رویداد را نیز بررسی می‌کند
    /// </summary>
    private MethodInfo? FindOnMethod(IDomainEvent @event)
    {
        Type eventType = @event.GetType();
        Type aggregateType = GetType();

        // 1. ابتدا متد دقیق را جستجو کن
        var method = aggregateType.GetMethod(
            "On",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            new[] { eventType },
            null);

        // 2. اگر پیدا نشد، والدین رویداد را بررسی کن
        if (method == null)
        {
            Type? baseType = eventType.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                method = aggregateType.GetMethod(
                    "On",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new[] { baseType },
                    null);

                if (method != null) break;
                baseType = baseType.BaseType;
            }
        }

        return method;
    }
}