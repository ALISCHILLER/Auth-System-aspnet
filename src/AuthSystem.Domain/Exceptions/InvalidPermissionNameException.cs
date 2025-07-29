using System;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// استثنا برای زمانی که نام مجوز نامعتبر است (مثلاً خالی یا null)
    /// </summary>
    public class InvalidPermissionNameException : DomainException
    {
        /// <summary>
        /// نام مجوز که باعث بروز خطا شده
        /// </summary>
        public string? InvalidName { get; }

        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public InvalidPermissionNameException()
            : base("نام مجوز نمی‌تواند خالی یا فقط شامل فاصله باشد")
        {
            InvalidName = null;
        }

        /// <summary>
        /// سازنده با نام مجوز نامعتبر
        /// </summary>
        /// <param name="invalidName">نام مجوز نامعتبر</param>
        public InvalidPermissionNameException(string? invalidName)
            : base($"نام مجوز '{invalidName}' نمی‌تواند خالی یا فقط شامل فاصله باشد")
        {
            InvalidName = invalidName;
        }

        // این سازنده را حذف کردم چون با سازنده بالا هم‌امضا بود
        // اگر نیاز به سازنده با پیام اختصاصی داشتید، می‌توانید نام پارامتر را تغییر دهید یا تعداد پارامترها را بیشتر کنید
    }
}
