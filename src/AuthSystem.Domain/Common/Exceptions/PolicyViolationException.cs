// File: AuthSystem.Domain/Common/Exceptions/PolicyViolationException.cs
using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common.Policies;

namespace AuthSystem.Domain.Common.Exceptions
{
    /// <summary>
    /// استثنای نقض سیاست‌ها در لایهٔ دامنه
    /// - زمانی پرتاب می‌شود که PolicyResult اجازه ندهد
    /// - این نسخه از لیست کامل خطاها پشتیبانی می‌کند
    /// </summary>
    public class PolicyViolationException : DomainException
    {
        /// <summary>
        /// لیست خطاها (پیام و کد) از تمام سیاست‌های نقض شده
        /// </summary>
        public IReadOnlyList<(string Message, string ErrorCode)> Errors { get; }

        /// <summary>
        /// سازنده برای ایجاد استثنا با لیست خطاها
        /// </summary>
        public PolicyViolationException(IReadOnlyList<(string Message, string ErrorCode)> errors)
            : base(CreateMessage(errors), GetFirstErrorCode(errors))
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        /// <summary>
        /// سازنده برای ایجاد استثنا با یک خطا
        /// </summary>
        public PolicyViolationException(string message, string errorCode)
            : this(new List<(string, string)> { (message, errorCode) })
        {
        }

        /// <summary>
        /// ایجاد پیام خطا از لیست خطاها
        /// </summary>
        private static string CreateMessage(IReadOnlyList<(string Message, string ErrorCode)> errors)
        {
            return "نقض سیاست‌های زیر رخ داده است: " +
                   string.Join(" | ", errors.Select(e => $"[{e.ErrorCode}] {e.Message}"));
        }

        /// <summary>
        /// دریافت کد خطا از اولین خطا در لیست
        /// </summary>
        private static string GetFirstErrorCode(IReadOnlyList<(string Message, string ErrorCode)> errors)
        {
            return errors.Count > 0 ? errors[0].ErrorCode : "POLICY_VIOLATION";
        }
    }
}