// AuthSystem.Domain/Exceptions/BusinessRuleValidationException.cs
using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای نقض قوانین کسب‌وکار
/// </summary>
[Serializable]
public class BusinessRuleValidationException : DomainException
{
    /// <summary>
    /// قانون نقض شده
    /// </summary>
    public string RuleName { get; }

    public BusinessRuleValidationException(string message, string errorCode, string? ruleName = null)
        : base(message, errorCode)
    {
        RuleName = ruleName ?? "UnknownRule";
        WithDetail(nameof(RuleName), RuleName);
    }

    public BusinessRuleValidationException(IBusinessRule brokenRule)
        : this(brokenRule.Message, brokenRule.ErrorCode, brokenRule.GetType().Name)
    {
    }

    public BusinessRuleValidationException(IAsyncBusinessRule brokenRule)
        : this(brokenRule.Message, brokenRule.ErrorCode, brokenRule.GetType().Name)
    {
    }
}