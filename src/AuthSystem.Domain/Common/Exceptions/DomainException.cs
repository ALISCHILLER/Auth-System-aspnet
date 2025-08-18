// File: AuthSystem.Domain/Common/Exceptions/DomainException.cs
using System;

namespace AuthSystem.Domain.Common.Exceptions
{
    /// <summary>
    /// استثنای پایه برای تمامی خطاهای سطح Domain
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// کد خطا برای پردازش‌های بعدی
        /// </summary>
        public string? ErrorCode { get; }

        public DomainException(string message, string? errorCode = null) : base(message)
        {
            ErrorCode = errorCode;
        }

        public DomainException(string message, string? errorCode, Exception? innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}