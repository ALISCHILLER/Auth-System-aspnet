namespace AuthSystem.Domain.Common.Policies;

/// <summary>
/// اینترفیس پایه برای تمام سیاست‌ها
/// </summary>
public interface IPolicy<in TContext>
{
    /// <summary>
    /// ارزیابی سیاست
    /// </summary>
    PolicyResult Evaluate(TContext context);
}