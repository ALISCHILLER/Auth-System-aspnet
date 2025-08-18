// File: AuthSystem.Domain/Common/Exceptions/AggregateBusinessRuleValidationException.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Domain.Common.Exceptions
{
    /// <summary>
    /// استثنای نقض چندین قانون کسب‌وکار به صورت همزمان
    /// </summary>
    public class AggregateBusinessRuleValidationException : DomainException
    {
        /// <summary>
        /// لیست خطاها (پیام و کد)
        /// </summary>
        public IReadOnlyList<(string Message, string ErrorCode)> Errors { get; }

        public AggregateBusinessRuleValidationException(IEnumerable<(string Message, string ErrorCode)> errors)
            : base(CreateMessage(errors), GetFirstErrorCode(errors))
        {
            Errors = errors.ToList().AsReadOnly();
        }

        private static string CreateMessage(IEnumerable<(string Message, string ErrorCode)> errors)
        {
            return "چندین قانون کسب‌وکار نقض شده است: " +
                   string.Join(" | ", errors.Select(e => $"[{e.ErrorCode}] {e.Message}"));
        }

        private static string? GetFirstErrorCode(IEnumerable<(string Message, string ErrorCode)> errors)
        {
            return errors.FirstOrDefault().ErrorCode;
        }
    }
}