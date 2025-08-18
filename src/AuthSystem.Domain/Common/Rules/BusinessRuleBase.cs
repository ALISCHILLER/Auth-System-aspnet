// File: AuthSystem.Domain/Common/Rules/BusinessRuleBase.cs
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Domain.Common.Rules
{
    /// <summary>
    /// پایهٔ ساده برای قواعد کسب‌وکار همگام
    /// - متد Check() برای پرتاب استثنای دامنه هنگام نقض قاعده
    /// </summary>
    public abstract class BusinessRuleBase : IBusinessRule
    {
        public abstract string Message { get; }
        public abstract string ErrorCode { get; }

        /// <summary>منطق تشخیص نقض قاعده</summary>
        protected abstract bool Broken();

        public bool IsBroken() => Broken();

        /// <summary>بررسی و پرتاب خطا در صورت نقض قاعده</summary>
        public void Check()
        {
            if (IsBroken())
                throw new BusinessRuleValidationException(Message, ErrorCode);
        }
    }
}