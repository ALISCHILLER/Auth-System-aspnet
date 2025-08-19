using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Policies;

/// <summary>
/// کلاس برای ارزیابی سیاست‌ها
/// </summary>
public class PolicyEvaluator
{
    /// <summary>
    /// ارزیابی یک سیاست
    /// </summary>
    public PolicyResult Evaluate<TContext>(IPolicy<TContext> policy, TContext context)
    {
        return policy.Evaluate(context);
    }

    /// <summary>
    /// ارزیابی ناهمزمان یک سیاست
    /// </summary>
    public async Task<PolicyResult> EvaluateAsync<TContext>(IAsyncPolicy<TContext> policy, TContext context)
    {
        return await policy.EvaluateAsync(context);
    }

    /// <summary>
    /// ارزیابی چندین سیاست
    /// </summary>
    public PolicyResult EvaluateAll<TContext>(IEnumerable<IPolicy<TContext>> policies, TContext context)
    {
        var results = policies
            .Select(policy => policy.Evaluate(context))
            .ToList();

        return new PolicyResult
        {
            IsSatisfied = results.All(r => r.IsSatisfied),
            Messages = results.SelectMany(r => r.Messages).ToList()
        };
    }

    /// <summary>
    /// ارزیابی ناهمزمان چندین سیاست
    /// </summary>
    public async Task<PolicyResult> EvaluateAllAsync<TContext>(
        IEnumerable<IAsyncPolicy<TContext>> policies,
        TContext context)
    {
        var results = await Task.WhenAll(
            policies.Select(policy => policy.EvaluateAsync(context))
        );

        return new PolicyResult
        {
            IsSatisfied = results.All(r => r.IsSatisfied),
            Messages = results.SelectMany(r => r.Messages).ToList()
        };
    }
}