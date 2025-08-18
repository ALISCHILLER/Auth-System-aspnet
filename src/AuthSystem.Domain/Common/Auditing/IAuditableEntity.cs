// File: AuthSystem.Domain/Common/Auditing/IAuditableEntity.cs
using System;

namespace AuthSystem.Domain.Common.Auditing
{
    /// <summary>
    /// قرارداد «حسابرسی پایه» برای موجودیت‌ها
    /// - تاریخ ایجاد و آخرین به‌روزرسانی
    /// - پیاده‌سازی ویژگی‌ها معمولاً در BaseEntity انجام می‌شود
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>تاریخ ایجاد (UTC)</summary>
        DateTime CreatedAt { get; }

        /// <summary>تاریخ آخرین به‌روزرسانی (UTC)</summary>
        DateTime? UpdatedAt { get; }
    }
}