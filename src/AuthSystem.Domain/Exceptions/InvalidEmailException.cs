using System;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// استثنا برای زمانی که آدرس ایمیل نامعتبر است
    /// </summary>
    public class InvalidEmailException : DomainException
    {
        /// <summary>
        /// آدرس ایمیلی که باعث بروز خطا شده
        /// </summary>
        public string? InvalidEmail { get; }

        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public InvalidEmailException()
            : base("فرمت آدرس ایمیل نامعتبر است")
        {
        }

        /// <summary>
        /// سازنده با پیام خطا (فقط پیام، بدون ایمیل)
        /// </summary>
        /// <param name="message">پیام خطا</param>
        public InvalidEmailException(string message, bool isCustomMessageOnly)
            : base(message)
        {
            // این سازنده فقط پیام را می‌گیرد و آدرس ایمیل را تنظیم نمی‌کند
        }

        /// <summary>
        /// سازنده با آدرس ایمیل نامعتبر و پیام خطا
        /// </summary>
        /// <param name="invalidEmail">آدرس ایمیل نامعتبر</param>
        /// <param name="message">پیام خطا (اختیاری)</param>
        public InvalidEmailException(string invalidEmail, string? message = null)
            : base(message ?? $"فرمت آدرس ایمیل '{invalidEmail}' نامعتبر است")
        {
            InvalidEmail = invalidEmail;
        }

        /// <summary>
        /// متد ساخت استثنا برای حالت ایمیل نامعتبر با پیام پیش‌فرض
        /// </summary>
        /// <param name="email">ایمیل نامعتبر</param>
        /// <returns>شیء InvalidEmailException</returns>
        public static InvalidEmailException ForInvalidEmail(string email)
            => new InvalidEmailException(email);
    }
}
