using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// Exception برای زمانی که توکن تازه‌سازی نامعتبر یا منقضی شده است
    /// </summary>

    public class InvalidRefreshTokenException : DomainException
    {
        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public InvalidRefreshTokenException() : base("توکن تازه‌سازی نامعتبر یا منقضی شده است.")
        {
        }

        /// <summary>
        /// سازنده با پیام سفارشی
        /// </summary>
        /// <param name="message">پیام خطا</param>
        public InvalidRefreshTokenException(string message) : base(message)
        {
        }
    }
}