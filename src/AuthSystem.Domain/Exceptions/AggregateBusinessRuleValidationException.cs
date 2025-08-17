// AuthSystem.Domain/Exceptions/AggregateBusinessRuleValidationException.cs
using System;
using System.Collections.Generic;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای نقض چندین قانون کسب‌وکار
/// </summary>
[Serializable]
public class AggregateBusinessRuleValidationException : DomainException
{
    /// <summary>
    /// لیست قوانین نقض شده
    /// </summary>
    public IReadOnlyList<(string Message, string ErrorCode)> BrokenRules { get; }

    public AggregateBusinessRuleValidationException(
        IEnumerable<(string Message, string ErrorCode)> brokenRules)
        : base(
            $"چندین قانون کسب‌وکار نقض شده است: {string.Join(", ", brokenRules)}",
            "MULTIPLE_BUSINESS_RULES_BROKEN")
    {
        BrokenRules = new List<(string, string)>(brokenRules).AsReadOnly();
        WithDetail(nameof(BrokenRules), BrokenRules);
    }
}