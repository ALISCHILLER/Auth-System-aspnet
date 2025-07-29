using System;

namespace AuthSystem.Domain.Common;

/// <summary>
/// رابط پایه برای تمام رویدادهای دامنه
/// هر رویدادی که در سیستم اتفاق می‌افتد باید این اینترفیس را پیاده‌سازی کند
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// زمان وقوع رویداد
    /// همیشه از زمان جهانی (UTC) استفاده می‌کنیم تا از مشکلات مربوط به منطقه زمانی جلوگیری شود
    /// این ویژگی اطمینان می‌دهد که تمام رویدادها زمان یکسانی برای ثبت دارند
    /// </summary>
    DateTime OccurredOn { get; }
}