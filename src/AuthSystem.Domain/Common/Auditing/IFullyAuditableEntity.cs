namespace AuthSystem.Domain.Common.Auditing;

/// <summary>
/// اینترفیس برای موجودیت‌های کاملاً قابل حسابرسی
/// شامل تمام فیلدهای حسابرسی
/// </summary>
public interface IFullyAuditableEntity : IAuditableEntity
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