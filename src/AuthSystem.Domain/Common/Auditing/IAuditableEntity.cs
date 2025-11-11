namespace AuthSystem.Domain.Common.Auditing;

/// <summary>
/// اینترفیس برای موجودیت‌های قابل حسابرسی
/// شامل فیلدهای پایه برای حسابرسی
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// تاریخ ایجاد موجودیت (UTC)
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// تاریخ آخرین به‌روزرسانی (UTC)
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    /// شناسه کاربر ایجاد کننده
    /// </summary>
    Guid? CreatedBy { get; }

    /// <summary>
    /// شناسه کاربر ویرایش کننده
    /// </summary>
    Guid? UpdatedBy { get; }
}