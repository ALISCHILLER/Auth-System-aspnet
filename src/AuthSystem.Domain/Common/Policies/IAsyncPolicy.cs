// File: AuthSystem.Domain/Common/Policies/IAsyncPolicy.cs
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Policies
{
    /// <summary>
    /// قرارداد Policy ناهمگام
    /// - برای مواردی که ارزیابی به عملیات async نیاز دارد
    /// </summary>
    public interface IAsyncPolicy<in TContext>
    {
        Task<PolicyResult> EvaluateAsync(TContext context, CancellationToken cancellationToken = default);
    }
}