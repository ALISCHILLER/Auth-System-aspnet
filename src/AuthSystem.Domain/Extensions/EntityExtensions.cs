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
            entity.MarkAsUpdated();
        }

        /// <summary>
        /// ثبت زمان بروزرسانی
        /// </summary>
        public static void Touch<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            entity.MarkAsUpdated();
        }
    }
}