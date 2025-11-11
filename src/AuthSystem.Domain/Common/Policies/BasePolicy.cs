namespace AuthSystem.Domain.Common.Policies;

/// <summary>
/// کلاس پایه برای تمام سیاست‌ها
/// </summary>
public abstract class BasePolicy<TContext> : IPolicy<TContext>
{
    /// <summary>
    /// ارزیابی سیاست
    /// </summary>
    public abstract PolicyResult Evaluate(TContext context);

    /// <summary>
    /// ارزیابی ناهمزمان سیاست
    /// </summary>
    public virtual Task<PolicyResult> EvaluateAsync(TContext context)
    {
        return Task.FromResult(Evaluate(context));
    }
}