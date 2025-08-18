// File: AuthSystem.Domain/Common/Policies/IRateLimiter.cs
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Policies
{
    /// <summary>
    /// قرارداد Rate Limiter
    /// - کنترل تعداد درخواست‌ها در بازه زمانی مشخص
    /// </summary>
    public interface IRateLimiter
    {
        /// <summary>
        /// آیا اجازه انجام عملیات را می‌دهد؟
        /// </summary>
        Task<bool> AllowAsync(string key, int permits = 1, CancellationToken cancellationToken = default);
    }
}