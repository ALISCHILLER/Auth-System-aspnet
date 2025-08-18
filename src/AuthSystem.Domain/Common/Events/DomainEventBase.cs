// File: AuthSystem.Domain/Common/Events/DomainEventBase.cs
using System;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Events
{
    /// <summary>
    /// کلاس پایه برای پیاده‌سازی رویدادها
    /// - تولید EventId/CorrelationId
    /// - ثبت زمان وقوع به UTC
    /// </summary>
    public abstract class DomainEventBase : IDomainEvent
    {
        public Guid EventId { get; }
        public DateTime OccurredOn { get; }
        public string? TriggeredBy { get; }
        public Guid CorrelationId { get; }
        public string EventType => GetType().Name;
        public virtual int EventVersion => 1;

        protected DomainEventBase(string? triggeredBy = null, Guid? correlationId = null)
        {
            EventId = Guid.NewGuid();
            OccurredOn = DomainClock.UtcNow;
            TriggeredBy = triggeredBy;
            CorrelationId = correlationId ?? Guid.NewGuid();
        }

        protected DomainEventBase(Guid eventId, DateTime occurredOn, string? triggeredBy, Guid correlationId)
        {
            EventId = eventId;
            OccurredOn = occurredOn;
            TriggeredBy = triggeredBy;
            CorrelationId = correlationId;
        }

        public virtual DomainEventMetadata GetMetadata() => new()
        {
            EventId = EventId,
            EventType = EventType,
            EventVersion = EventVersion,
            OccurredOn = OccurredOn,
            TriggeredBy = TriggeredBy,
            CorrelationId = CorrelationId
        };
    }
}