using System;
using System.Collections.Generic;
using System.Linq;

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
        var result = new PolicyResult { IsSatisfied = true };
        if (message != null)
            result.Messages.Add(message);
        return result;
    }

    /// <summary>
    /// ایجاد نتیجه ناموفق
    /// </summary>
    public static PolicyResult Failure(string message)
    {
        var result = new PolicyResult { IsSatisfied = false };
        result.Messages.Add(message);
        return result;
    }

    /// <summary>
    /// ترکیب چندین نتیجه
    /// </summary>
    public static PolicyResult Combine(IEnumerable<PolicyResult> results)
    {
        var result = new PolicyResult { IsSatisfied = results.All(r => r.IsSatisfied) };

        foreach (var r in results)
        {
            result.Messages.AddRange(r.Messages);
        }

        return result;
    }
}