using System;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// استثنا برای زمانی که شماره تلفن نامعتبر است
    /// </summary>
    public class InvalidPhoneNumberException : DomainException
    {
        /// <summary>
        /// شماره تلفنی که باعث بروز خطا شده
        /// </summary>
        public string? InvalidPhoneNumber { get; }

        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public InvalidPhoneNumberException()
            : base("فرمت شماره تلفن نامعتبر است")
        {
        }

        /// <summary>
        /// سازنده با پیام خطا (تنها پیام)
        /// </summary>
        /// <param name="message">پیام خطا</param>
        public InvalidPhoneNumberException(string message, bool isMessageOnly)
            : base(message)
        {
            // این سازنده فقط پیام خطا را می‌پذیرد و شماره را تنظیم نمی‌کند
        }

        /// <summary>
        /// سازنده با شماره تلفن نامعتبر و پیام خطا (پیام اختیاری)
        /// </summary>
        /// <param name="invalidPhoneNumber">شماره تلفن نامعتبر</param>
        /// <param name="message">پیام خطا</param>
        public InvalidPhoneNumberException(string invalidPhoneNumber, string? message = null)
            : base(message ?? $"فرمت شماره تلفن '{invalidPhoneNumber}' نامعتبر است")
        {
            InvalidPhoneNumber = invalidPhoneNumber;
        }
    }
}
