using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// Exception برای زمانی که کاربر یافت نشود
    /// </summary>
    public class UserNotFoundException : DomainException
    {
        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public UserNotFoundException() : base("کاربر یافت نشد.")
        {
        }

        /// <summary>
        /// سازنده با پیام سفارشی
        /// </summary>
        /// <param name="message">پیام خطا</param>
        public UserNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// سازنده با شناسه کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر یافت نشده</param>
        public UserNotFoundException(Guid userId) : base($"کاربر با شناسه {userId} یافت نشد.")
        {
        }
    }
}