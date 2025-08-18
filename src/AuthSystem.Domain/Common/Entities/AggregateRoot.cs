// File: AuthSystem.Domain/Common/Entities/AggregateRoot.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Common.Entities
{
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

        /// <summary>رویدادهای معلق دامنه (فقط خواندنی)</summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>نسخه برای کنترل Concurrency</summary>
        public int Version { get; protected set; } = 0;

        /// <summary>افزودن رویداد دامنه به صف انتشار</summary>
        protected void AddDomainEvent(IDomainEvent @event)
        {
            _domainEvents.Add(@event);
        }

        /// <summary>افزودن شرطی رویداد دامنه</summary>
        protected void AddDomainEventIf(bool condition, Func<IDomainEvent> eventFactory)
        {
            if (condition)
                AddDomainEvent(eventFactory());
        }

        /// <summary>حذف یک رویداد دامنه</summary>
        public void RemoveDomainEvent(IDomainEvent @event)
        {
            _domainEvents.Remove(@event);
        }

        /// <summary>پاک‌سازی همهٔ رویدادهای معلق</summary>
        public void ClearDomainEvents() => _domainEvents.Clear();

        /// <summary>آیا رویداد خاصی وجود دارد؟</summary>
        public bool HasDomainEvent<TEvent>() where TEvent : IDomainEvent
            => _domainEvents.Any(e => e is TEvent);

        /// <summary>دریافت همهٔ رویدادهای نوع خاص</summary>
        public IEnumerable<TEvent> GetDomainEvents<TEvent>() where TEvent : IDomainEvent
            => _domainEvents.OfType<TEvent>();

        /// <summary>افزایش نسخه برای Concurrency Control</summary>
        protected void IncrementVersion() => Version++;

        /// <summary>علامت‌گذاری به‌عنوان به‌روزرسانی‌شده + افزایش نسخه</summary>
        protected override void MarkAsUpdated()
        {
            base.MarkAsUpdated();
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
                throw new BusinessRuleValidationException(rule.Message, rule.ErrorCode);
        }

        /// <summary>
        /// بررسی قانون ناهمزمان (Async)
        /// </summary>
        protected static async Task CheckRuleAsync(IAsyncBusinessRule rule)
        {
            if (rule is null)
                throw new ArgumentNullException(nameof(rule));

            if (await rule.IsBrokenAsync().ConfigureAwait(false))
                throw new BusinessRuleValidationException(rule.Message, rule.ErrorCode);
        }

        /// <summary>
        /// بررسی چندین قانون Async به صورت موازی
        /// </summary>
        protected static async Task CheckRulesAsync(params IAsyncBusinessRule[] rules)
        {
            if (rules == null || rules.Length == 0)
                return;

            var tasks = rules.Select(async r =>
            {
                var broken = await r.IsBrokenAsync().ConfigureAwait(false);
                return (Rule: r, Broken: broken);
            });

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            var broken = results
                .Where(x => x.Broken)
                .Select(x => (x.Rule.Message, x.Rule.ErrorCode))
                .ToList();

            if (broken.Any())
                throw new AggregateBusinessRuleValidationException(broken);
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

        /// <summary>
        /// اعمال و افزودن رویداد به صف (برای انتشار بعدی)
        /// - ابتدا حالت را تغییر می‌دهد
        /// - سپس رویداد را برای انتشار اضافه می‌کند
        /// - نسخه را افزایش می‌دهد
        /// </summary>
        protected void ApplyRaise(IDomainEvent @event)
        {
            Apply(@event);          // تغییر حالت
            AddDomainEvent(@event); // افزودن برای انتشار
            IncrementVersion();     // افزایش نسخه
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
                Apply(e); // فقط اعمال رویداد بدون افزودن به DomainEvents
                Version++;
            }
        }
    }
}