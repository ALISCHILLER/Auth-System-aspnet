// File: AuthSystem.Domain/Common/Auditing/IFullyAuditableEntity.cs
namespace AuthSystem.Domain.Common.Auditing
{
    /// <summary>
    /// قرارداد «حسابرسی کامل»
    /// - شامل ایجاد/به‌روزرسانی + حذف نرم + شناسه‌های کاربری
    /// - معمولاً در Aggregateهای حساس به ردیابی استفاده می‌شود
    /// </summary>
    public interface IFullyAuditableEntity : IAuditableEntity, ISoftDeletableEntity
    {
        /// <summary>کاربر ایجادکننده (اختیاری)</summary>
        string? CreatedBy { get; }

        /// <summary>کاربر آخرین به‌روزرسانی‌کننده (اختیاری)</summary>
        string? UpdatedBy { get; }
    }
}