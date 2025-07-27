using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// Exception پایه برای تمامی Exceptionهای دامنه
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// سازنده با پیام
        /// </summary>
        /// <param name="message">پیام خطا</param>
        protected DomainException(string message) : base(message)
        {
        }
    }
}