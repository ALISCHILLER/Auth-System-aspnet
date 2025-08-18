// File: AuthSystem.Domain/Common/Rules/IAsyncBusinessRule.cs
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Rules
{
    /// <summary>
    /// قرارداد «قاعده کسب‌وکار» ناهمگام
    /// - برای مواردی که بررسی قانون به عملیات async نیاز دارد
    /// - فقط قرارداد دامنه است؛ بدون I/O
    /// </summary>
    public interface IAsyncBusinessRule
    {
        /// <summary>پیام خطا در صورت نقض قاعده</summary>
        string Message { get; }

        /// <summary>کد خطا برای پردازش‌های بعدی</summary>
        string ErrorCode { get; }

        /// <summary>آیا قاعده نقض شده است؟</summary>
        Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default);
    }
}