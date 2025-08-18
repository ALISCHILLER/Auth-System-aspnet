// File: AuthSystem.Domain/Common/Events/IDomainEvent.cs
using System;

namespace AuthSystem.Domain.Common.Events
{
    /// <summary>
    /// قرارداد پایه رویدادهای دامنه
    /// - همهٔ رویدادها باید این اینترفیس را پیاده‌سازی کنند
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>شناسه یکتای رویداد</summary>
        Guid EventId { get; }

        /// <summary>زمان وقوع (UTC)</summary>
        DateTime OccurredOn { get; }

        /// <summary>شناسه/نام کاربری که رویداد را ایجاد کرده (اختیاری)</summary>
        string? TriggeredBy { get; }

        /// <summary>Correlation Id برای ردیابی توزیع‌شده</summary>
        Guid CorrelationId { get; }

        /// <summary>نام/نوع رویداد جهت دسته‌بندی/سریال‌سازی</summary>
        string EventType { get; }

        /// <summary>نسخهٔ رویداد جهت سازگاری</summary>
        int EventVersion { get; }

        /// <summary>بازیابی متادیتای رویداد</summary>
        DomainEventMetadata GetMetadata();
    }
}