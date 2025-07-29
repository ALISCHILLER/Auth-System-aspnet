using System;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// استثنا برای زمانی که کاربر مجوز کافی برای انجام عملیات ندارد
    /// </summary>
    public class InsufficientPermissionsException : DomainException
    {
        /// <summary>
        /// نام عملیات یا مجوز مورد نیاز
        /// </summary>
        public string RequiredPermission { get; }

        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public Guid? UserId { get; }

        /// <summary>
        /// سازنده با نام مجوز
        /// </summary>
        /// <param name="requiredPermission">نام مجوز مورد نیاز</param>
        public InsufficientPermissionsException(string requiredPermission)
            : base($"مجوز کافی برای انجام عملیات {requiredPermission} وجود ندارد")
        {
            RequiredPermission = requiredPermission;
        }

        /// <summary>
        /// سازنده با شناسه کاربر و نام مجوز
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="requiredPermission">نام مجوز مورد نیاز</param>
        public InsufficientPermissionsException(Guid userId, string requiredPermission)
            : base($"کاربر با شناسه {userId} مجوز کافی برای انجام عملیات {requiredPermission} ندارد")
        {
            UserId = userId;
            RequiredPermission = requiredPermission;
        }

        /// <summary>
        /// سازنده عمومی با پیام خطا
        /// </summary>
        /// <param name="message">پیام خطا</param>
        public InsufficientPermissionsException(string message, bool isCustomMessage)
            : base(message)
        {
            // این سازنده وقتی استفاده می‌شود که بخواهیم پیام دلخواه بفرستیم
            // پارامتر دوم فقط برای تمایز سازنده است
        }
    }
}
