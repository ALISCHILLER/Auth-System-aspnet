using System.Collections.Generic;

namespace AuthSystem.Domain.Common.Policies;

/// <summary>
/// کلاس برای نتیجه ارزیابی سیاست‌ها
/// </summary>
public class PolicyResult
{
    /// <summary>
    /// آیا سیاست برآورده شده است
    /// </summary>
    public bool IsSatisfied { get; set; }

    /// <summary>
    /// پیام‌های نتیجه
    /// </summary>
    public List<string> Messages { get; } = new List<string>();

    /// <summary>
    /// ایجاد نتیجه موفق
    /// </summary>
    public static PolicyResult Success(string message = null)
    {
        return new PolicyResult
        {
            IsSatisfied = true,
            Messages = message != null ? new List<string> { message } : new List<string>()
        };
    }

    /// <summary>
    /// ایجاد نتیجه ناموفق
    /// </summary>
    public static PolicyResult Failure(string message)
    {
        return new PolicyResult
        {
            IsSatisfied = false,
            Messages = new List<string> { message }
        };
    }

    /// <summary>
    /// ترکیب چندین نتیجه
    /// </summary>
    public static PolicyResult Combine(IEnumerable<PolicyResult> results)
    {
        return new PolicyResult
        {
            IsSatisfied = results.All(r => r.IsSatisfied),
            Messages = results.SelectMany(r => r.Messages).ToList()
        };
    }
}