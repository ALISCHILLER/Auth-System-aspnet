using System;

namespace AuthSystem.Domain.Common.Events;

/// <summary>
/// اینترفیس پایه برای تمام رویدادهای دامنه
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// تاریخ و زمان وقوع رویداد
    /// </summary>
    DateTime OccurredOn { get; }

    /// <summary>
    /// شناسه منحصر به فرد رویداد
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// آیا رویداد به صورت ناهمزمان پردازش شود
    /// </summary>
    bool IsAsync { get; }

    /// <summary>
    /// آیا رویداد منتشر شده است
    /// </summary>
    bool IsPublished { get; }

    /// <summary>
    /// علامت‌گذاری رویداد به عنوان منتشر شده
    /// </summary>
    void MarkAsPublished();
}