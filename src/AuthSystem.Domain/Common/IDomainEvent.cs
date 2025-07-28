using System;

namespace AuthSystem.Domain.Common
{
    /// <summary>
    /// رابط پایه برای تمام رویدادهای دامنه
    /// هر رویدادی که در سیستم اتفاق می‌افتد باید این اینترفیس را پیاده‌سازی کند
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// زمان وقوع رویداد
        /// همیشه از UTC استفاده می‌کنیم برای جلوگیری از مشکلات زمانی
        /// </summary>
        DateTime OccurredOn { get; }
    }
}