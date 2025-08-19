using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Events;

/// <summary>
/// کلاس پایه برای رویدادهای دامنه
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    /// <summary>
    /// تاریخ و زمان وقوع رویداد
    /// </summary>
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    /// <summary>
    /// متادیتای اضافی رویداد
    /// </summary>
    public Dictionary<string, object> Metadata { get; } = new Dictionary<string, object>();

    /// <summary>
    /// شناسه منحصر به فرد رویداد
    /// </summary>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <summary>
    /// آیا رویداد به صورت ناهمزمان پردازش شود
    /// </summary>
    public virtual bool IsAsync => false;

    /// <summary>
    /// آیا رویداد باید منتشر شود
    /// </summary>
    public bool IsPublished { get; private set; }

    /// <summary>
    /// علامت‌گذاری رویداد به عنوان منتشر شده
    /// </summary>
    public void MarkAsPublished()
    {
        IsPublished = true;
    }

    /// <summary>
    /// افزودن متادیتای سفارشی
    /// </summary>
    public void AddMetadata(string key, object value)
    {
        Metadata[key] = value;
    }
}