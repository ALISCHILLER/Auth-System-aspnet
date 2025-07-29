using System;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Entities;

namespace AuthSystem.Domain.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// مقداردهی اولیه به موجودیت جدید
        /// </summary>
        public static void InitializeEntity<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            entity.SetId(Guid.NewGuid());
            entity.SetCreatedAt(DateTime.UtcNow);
            entity.SetUpdatedAt(DateTime.UtcNow);
            entity.SetIsActive(true);
            entity.SetIsDeleted(false);
        }

        /// <summary>
        /// ثبت زمان بروزرسانی
        /// </summary>
        public static void Touch<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            entity.SetUpdatedAt(DateTime.UtcNow);
        }

        /// <summary>
        /// غیرفعال کردن موجودیت
        /// </summary>
        public static void Deactivate<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            entity.SetIsActive(false);
            entity.Touch();
        }

        /// <summary>
        /// فعال‌سازی موجودیت
        /// </summary>
        public static void Activate<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            entity.SetIsActive(true);
            entity.Touch();
        }

        /// <summary>
        /// حذف منطقی موجودیت
        /// </summary>
        public static void SoftDelete<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            entity.SetIsDeleted(true);
            entity.Touch();
        }

        /// <summary>
        /// بازگردانی موجودیت حذف شده
        /// </summary>
        public static void Restore<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            entity.SetIsDeleted(false);
            entity.Touch();
        }

        /// <summary>
        /// بررسی فعال بودن موجودیت
        /// </summary>
        public static bool IsEntityActive<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            return entity.IsActive && !entity.IsDeleted;
        }
    }
}
