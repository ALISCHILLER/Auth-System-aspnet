using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Policies;

/// <summary>
/// Helper for evaluating policies individually or in batches.
/// </summary>
public sealed class PolicyEvaluator
{
  
    public PolicyResult Evaluate<TContext>(IPolicy<TContext> policy, TContext context)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        return policy.Evaluate(context);
    }

    public Task<PolicyResult> EvaluateAsync<TContext>(IAsyncPolicy<TContext> policy, TContext context)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        return policy.EvaluateAsync(context);
    }

    
    public PolicyResult EvaluateAll<TContext>(IEnumerable<IPolicy<TContext>> policies, TContext context)
    {
        if (policies is null) throw new ArgumentNullException(nameof(policies));
        var results = policies.Select(policy => policy.Evaluate(context)).ToArray();
        return PolicyResult.Combine(results);
    }

    public async Task<PolicyResult> EvaluateAllAsync<TContext>(IEnumerable<IAsyncPolicy<TContext>> policies, TContext context)
    {
        if (policies is null) throw new ArgumentNullException(nameof(policies));
        var policyArray = policies.ToArray();
        var results = await Task.WhenAll(policyArray.Select(policy => policy.EvaluateAsync(context))).ConfigureAwait(false);
        return PolicyResult.Combine(results);
    }
}