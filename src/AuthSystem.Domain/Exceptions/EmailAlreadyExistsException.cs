using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// Exception برای زمانی که ایمیل قبلاً ثبت شده است
    /// </summary>

    public class EmailAlreadyExistsException : DomainException
    {
        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public EmailAlreadyExistsException() : base("ایمیل قبلاً ثبت شده است.")
        {
        }

        /// <summary>
        /// سازنده با ایمیل تکراری
        /// </summary>
        /// <param name="email">ایمیل تکراری</param>
        public EmailAlreadyExistsException(string email) : base($"ایمیل {email} قبلاً ثبت شده است.")
        {
        }
    }
}