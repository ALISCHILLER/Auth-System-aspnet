using AuthSystem.Domain.Common.Abstractions;

namespace AuthSystem.Domain.Common.Exceptions;

/// <summary>
/// استثنا برای قوانین تجمعی
/// </summary>
public class AggregateBusinessRuleValidationException : DomainException
{
    /// <summary>
    /// لیست پیام‌های خطای قوانین
    /// </summary>
    public List<string> RuleMessages { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "AggregateRuleViolation";

    /// <summary>
    /// سازنده خصوصی برای Factory Method
    /// </summary>
    private AggregateBusinessRuleValidationException(List<string> ruleMessages)
        : base($"چندین قانون کسب‌وکار نقض شده‌اند: {string.Join(", ", ruleMessages)}")
    {
        RuleMessages = ruleMessages;
    }

    /// <summary>
    /// ایجاد استثنا برای چندین قانون شکسته شده
    /// </summary>
    public static AggregateBusinessRuleValidationException ForMultipleBrokenRules(
        IEnumerable<IBusinessRule> brokenRules)
    {
        var ruleMessages = brokenRules.Select(r => r.Message).ToList();
        return new AggregateBusinessRuleValidationException(ruleMessages);
    }

    /// <summary>
    /// ایجاد استثنا برای چندین قانون شکسته شده (ناهمزمان)
    /// </summary>
    public static async Task<AggregateBusinessRuleValidationException> ForMultipleBrokenRulesAsync(
        IEnumerable<IAsyncBusinessRule> brokenRules)
    {
        var ruleMessages = new List<string>();
        foreach (var rule in brokenRules)
        {
            ruleMessages.Add(await rule.GetMessageAsync());
        }
        return new AggregateBusinessRuleValidationException(ruleMessages);
    }
}