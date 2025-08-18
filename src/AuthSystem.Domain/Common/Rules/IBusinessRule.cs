// File: AuthSystem.Domain/Common/Rules/IBusinessRule.cs
namespace AuthSystem.Domain.Common.Rules
{
    /// <summary>
    /// قرارداد «قاعده کسب‌وکار» همگام
    /// - اجرای قوانین دامنه برای جلوگیری از وضعیت نامعتبر
    /// </summary>
    public interface IBusinessRule
    {
        /// <summary>پیام خطا در صورت نقض قاعده</summary>
        string Message { get; }

        /// <summary>کد خطا برای پردازش‌های بعدی</summary>
        string ErrorCode { get; }

        /// <summary>آیا قاعده نقض شده است؟</summary>
        bool IsBroken();
    }
}