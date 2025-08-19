using System.Collections.Generic;

namespace AuthSystem.Domain.Common.Rules;

/// <summary>
/// کلاس برای نتیجه ارزیابی قوانین
/// </summary>
public class RuleResult
{
    /// <summary>
    /// آیا قانون برآورده شده است
    /// </summary>
    public bool IsSatisfied { get; set; }

    /// <summary>
    /// پیام‌های نتیجه
    /// </summary>
    public List<string> Messages { get; } = new List<string>();

    /// <summary>
    /// ایجاد نتیجه موفق
    /// </summary>
    public static RuleResult Success(string message = null)
    {
        return new RuleResult
        {
            IsSatisfied = true,
            Messages = message != null ? new List<string> { message } : new List<string>()
        };
    }

    /// <summary>
    /// ایجاد نتیجه ناموفق
    /// </summary>
    public static RuleResult Failure(string message)
    {
        return new RuleResult
        {
            IsSatisfied = false,
            Messages = new List<string> { message }
        };
    }

    /// <summary>
    /// ترکیب چندین نتیجه
    /// </summary>
    public static RuleResult Combine(IEnumerable<RuleResult> results)
    {
        return new RuleResult
        {
            IsSatisfied = results.All(r => r.IsSatisfied),
            Messages = results.SelectMany(r => r.Messages).ToList()
        };
    }
}