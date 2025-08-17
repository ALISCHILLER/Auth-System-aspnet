using System;

namespace AuthSystem.Domain.Common;

/// <summary>
/// رابط برای موجودیت‌های قابل ممیزی
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// تاریخ ایجاد موجودیت
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// تاریخ آخرین به‌روزرسانی
    /// </summary>
    DateTime? UpdatedAt { get; }
}

/// <summary>
/// رابط گسترش‌یافته برای موجودیت‌های قابل ممیزی با اطلاعات کاربر
/// </summary>
public interface IFullyAuditableEntity : IAuditableEntity
{
    /// <summary>
    /// شناسه کاربر ایجادکننده
    /// </summary>
    string? CreatedBy { get; }

    /// <summary>
    /// شناسه کاربر آخرین ویرایش‌کننده
    /// </summary>
    string? UpdatedBy { get; }
}

/// <summary>
/// رابط برای موجودیت‌های قابل حذف منطقی
/// </summary>
public interface ISoftDeletableEntity
{
    /// <summary>
    /// آیا موجودیت حذف شده است
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// تاریخ حذف
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// شناسه کاربر حذف‌کننده
    /// </summary>
    string? DeletedBy { get; }
}
