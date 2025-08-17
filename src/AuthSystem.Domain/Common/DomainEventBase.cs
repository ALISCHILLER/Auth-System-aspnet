using System;

namespace AuthSystem.Domain.Common;

/// <summary>
/// کلاس پایه برای تمام رویدادهای دامنه
/// این کلاس اطلاعات مشترک تمام رویدادها را نگهداری می‌کند
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    /// <summary>
    /// شناسه یکتای رویداد برای ردیابی و Idempotency
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// زمان وقوع رویداد (UTC)
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// شناسه کاربری که رویداد را ایجاد کرده
    /// </summary>
    public string? TriggeredBy { get; }

    /// <summary>
    /// شناسه Correlation برای ردیابی در سیستم‌های توزیع‌شده
    /// </summary>
    public Guid CorrelationId { get; }

    /// <summary>
    /// نام نوع رویداد (برای سریال‌سازی)
    /// </summary>
    public string EventType => GetType().Name;

    /// <summary>
    /// نسخه رویداد (برای سازگاری با نسخه‌های قدیمی)
    /// </summary>
    public virtual int EventVersion => 1;

    /// <summary>
    /// سازنده با پارامترهای اختیاری
    /// </summary>
    protected DomainEventBase(
        string? triggeredBy = null,
        Guid? correlationId = null)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        TriggeredBy = triggeredBy;
        CorrelationId = correlationId ?? Guid.NewGuid();
    }

    /// <summary>
    /// سازنده برای بازسازی از Event Store
    /// </summary>
    protected DomainEventBase(
        Guid eventId,
        DateTime occurredOn,
        string? triggeredBy,
        Guid correlationId)
    {
        EventId = eventId;
        OccurredOn = occurredOn;
        TriggeredBy = triggeredBy;
        CorrelationId = correlationId;
    }

    /// <summary>
    /// دریافت metadata رویداد
    /// </summary>
    public virtual DomainEventMetadata GetMetadata()
    {
        return new DomainEventMetadata
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

/// <summary>
/// Metadata رویدادهای دامنه
/// </summary>
public class DomainEventMetadata
{
    public Guid EventId { get; init; }
    public string EventType { get; init; } = string.Empty;
    public int EventVersion { get; init; }
    public DateTime OccurredOn { get; init; }
    public string? TriggeredBy { get; init; }
    public Guid CorrelationId { get; init; }
}
