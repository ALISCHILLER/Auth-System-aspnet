using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Policies;

/// <summary>
/// اینترفیس برای سیاست‌های ناهمزمان
/// </summary>
public interface IAsyncPolicy<in TContext>
{
    /// <summary>
    /// ارزیابی ناهمزمان سیاست
    /// </summary>
    Task<PolicyResult> EvaluateAsync(TContext context);
}