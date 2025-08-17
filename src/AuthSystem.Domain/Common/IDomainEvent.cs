using System;

namespace AuthSystem.Domain.Common;

/// <summary>
/// رابط پایه برای تمام رویدادهای دامنه
/// هر رویدادی که در سیستم اتفاق می‌افتد باید این اینترفیس را پیاده‌سازی کند
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// شناسه یکتای رویداد
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// زمان وقوع رویداد (همیشه UTC)
    /// </summary>
    DateTime OccurredOn { get; }

    /// <summary>
    /// شناسه کاربری که رویداد را ایجاد کرده (اختیاری)
    /// </summary>
    string? TriggeredBy { get; }

    /// <summary>
    /// شناسه Correlation برای ردیابی در سیستم‌های توزیع‌شده
    /// </summary>
    Guid CorrelationId { get; }

    /// <summary>
    /// نوع رویداد برای سریال‌سازی و دسته‌بندی
    /// </summary>
    string EventType { get; }

    /// <summary>
    /// نسخه رویداد برای مدیریت تغییرات schema
    /// </summary>
    int EventVersion { get; }

    /// <summary>
    /// دریافت metadata رویداد
    /// </summary>
    DomainEventMetadata GetMetadata();
}
