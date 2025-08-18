// File: AuthSystem.Domain/Common/Exceptions/BusinessRuleValidationException.cs
namespace AuthSystem.Domain.Common.Exceptions
{
    /// <summary>
    /// استثنای نقض یک قانون کسب‌وکار ساده
    /// </summary>
    public class BusinessRuleValidationException : DomainException
    {
        /// <summary>
        /// کد خطای قانون
        /// </summary>
        public string ErrorCode { get; }

        public BusinessRuleValidationException(string message, string errorCode)
            : base(message, errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}