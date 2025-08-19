using System;
using System.Collections.Generic;
using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Common.Exceptions;

/// <summary>
/// استثنا برای قوانین کسب‌وکار
/// </summary>
public class BusinessRuleValidationException : DomainException
{
    /// <summary>
    /// پیام خطای قانون
    /// </summary>
    public string RuleMessage { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode { get; }

    /// <summary>
    /// سازنده خصوصی برای Factory Method
    /// </summary>
    private BusinessRuleValidationException(string message, string errorCode) : base(message)
    {
        RuleMessage = message;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// سازنده خصوصی برای Factory Method
    /// </summary>
    private BusinessRuleValidationException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        RuleMessage = message;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// ایجاد استثنا برای قانون شکسته شده
    /// </summary>
    public static BusinessRuleValidationException ForBrokenRule(IBusinessRule rule)
    {
        return new BusinessRuleValidationException(rule.Message, rule.ErrorCode);
    }

    /// <summary>
    /// ایجاد استثنا برای قانون شکسته شده (ناهمزمان)
    /// </summary>
    public static async Task<BusinessRuleValidationException> ForBrokenRuleAsync(IAsyncBusinessRule rule)
    {
        var message = await rule.GetMessageAsync();
        return new BusinessRuleValidationException(message, rule.ErrorCode);
    }
}