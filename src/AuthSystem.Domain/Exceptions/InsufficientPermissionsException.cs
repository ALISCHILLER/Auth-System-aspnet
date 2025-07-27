using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// Exception برای زمانی که کاربر مجوز لازم را ندارد
    /// </summary>

    public class InsufficientPermissionsException : DomainException
    {
        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public InsufficientPermissionsException() : base("شما مجوز لازم برای انجام این عملیات را ندارید.")
        {
        }

        /// <summary>
        /// سازنده با نام مجوز
        /// </summary>
        /// <param name="permission">نام مجوز لازم</param>
        public InsufficientPermissionsException(string permission) : base($"شما مجوز '{permission}' را ندارید.")
        {
        }

        /// <summary>
        /// سازنده با پیام و نام مجوز
        /// </summary>
        /// <param name="message">پیام خطا</param>
        /// <param name="permission">نام مجوز لازم</param>
        public InsufficientPermissionsException(string message, string permission) : base($"{message} شما مجوز '{permission}' را ندارید.")
        {
        }
    }
}