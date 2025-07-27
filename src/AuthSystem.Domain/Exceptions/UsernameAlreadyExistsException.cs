using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// Exception برای زمانی که نام کاربری قبلاً ثبت شده است
    /// </summary>
    public class UsernameAlreadyExistsException : DomainException
    {
        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public UsernameAlreadyExistsException() : base("نام کاربری قبلاً ثبت شده است.")
        {
        }

        /// <summary>
        /// سازنده با نام کاربری تکراری
        /// </summary>
        /// <param name="username">نام کاربری تکراری</param>
        public UsernameAlreadyExistsException(string username) : base($"نام کاربری {username} قبلاً ثبت شده است.")
        {
        }
    }
}