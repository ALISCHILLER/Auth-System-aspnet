// File: AuthSystem.Domain/Common/Rules/AsyncBusinessRuleBase.cs
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Domain.Common.Rules
{
    /// <summary>
    /// پایهٔ ساده برای قواعد کسب‌وکار ناهمگام
    /// - متد CheckAsync برای پرتاب استثنای دامنه در صورت نقض
    /// </summary>
    public abstract class AsyncBusinessRuleBase : IAsyncBusinessRule
    {
        public abstract string Message { get; }
        public abstract string ErrorCode { get; }

        /// <summary>منطق async تشخیص نقض قاعده</summary>
        protected abstract Task<bool> BrokenAsync(CancellationToken cancellationToken);

        public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
            => BrokenAsync(cancellationToken);

        /// <summary>بررسی و پرتاب خطا به‌صورت ناهمگام</summary>
        public async Task CheckAsync(CancellationToken cancellationToken = default)
        {
            if (await IsBrokenAsync(cancellationToken).ConfigureAwait(false))
                throw new BusinessRuleValidationException(Message, ErrorCode);
        }
    }
}