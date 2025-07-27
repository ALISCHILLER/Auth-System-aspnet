using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// Exception برای زمانی که اطلاعات ورود (نام کاربری/رمز عبور) نامعتبر است
    /// </summary>

    public class InvalidCredentialsException : DomainException
    {
        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public InvalidCredentialsException() : base("نام کاربری یا رمز عبور نامعتبر است.")
        {
        }

        /// <summary>
        /// سازنده با پیام سفارشی
        /// </summary>
        /// <param name="message">پیام خطا</param>
        public InvalidCredentialsException(string message) : base(message)
        {
        }
    }
}