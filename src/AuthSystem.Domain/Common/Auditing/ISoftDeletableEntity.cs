using System;

namespace AuthSystem.Domain.Common.Auditing;

/// <summary>
/// اینترفیس برای موجودیت‌های قابل حذف منطقی
/// </summary>
public interface ISoftDeletableEntity
{
    /// <summary>
    /// نشانه‌گذاری حذف منطقی
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// تاریخ حذف (در صورت حذف منطقی)
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// شناسه کاربر حذف کننده
    /// </summary>
    Guid? DeletedBy { get; }
}