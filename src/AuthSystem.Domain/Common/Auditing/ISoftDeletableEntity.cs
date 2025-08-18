// File: AuthSystem.Domain/Common/Auditing/ISoftDeletableEntity.cs
using System;

namespace AuthSystem.Domain.Common.Auditing
{
    /// <summary>
    /// قرارداد «حذف نرم» برای موجودیت‌ها
    /// - بدون حذف فیزیکی؛ فقط علامت حذف و زمان/کاربر ثبت می‌شود
    /// - پیاده‌سازی در لایهٔ دامنه صرفاً قرارداد است
    /// </summary>
    public interface ISoftDeletableEntity
    {
        /// <summary>آیا حذف نرم شده است</summary>
        bool IsDeleted { get; }

        /// <summary>زمان حذف (UTC)</summary>
        DateTime? DeletedAt { get; }

        /// <summary>شناسهٔ کاربری که حذف را انجام داده (اختیاری)</summary>
        string? DeletedBy { get; }
    }
}