// File: AuthSystem.Domain/Common/Policies/PolicyResult.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Domain.Common.Policies
{
    /// <summary>
    /// نتیجهٔ ارزیابی Policy
    /// - IsAllowed: اجازه داده شد؟
    /// - Errors: دلایل/پیام‌ها در صورت عدم اجازه
    /// </summary>
    public sealed class PolicyResult
    {
        /// <summary>
        /// آیا اجازه داده شد؟
        /// </summary>
        public bool IsAllowed { get; }

        /// <summary>
        /// لیست خطاها (پیام و کد) در صورت عدم اجازه
        /// </summary>
        public IReadOnlyList<PolicyError> Errors { get; }

        /// <summary>
        /// سازنده خصوصی
        /// </summary>
        private PolicyResult(bool isAllowed, List<PolicyError> errors)
        {
            IsAllowed = isAllowed;
            Errors = errors.AsReadOnly();
        }

        /// <summary>
        /// ساخت نتیجه موفق
        /// </summary>
        public static PolicyResult Allow() => new PolicyResult(true, new List<PolicyError>());

        /// <summary>
        /// ساخت نتیجه ناموفق با یک یا چند خطا
        /// </summary>
        public static PolicyResult Deny(params PolicyError[] errors)
        {
            return new PolicyResult(false, new List<PolicyError>(errors));
        }

        /// <summary>
        /// ساخت نتیجه ناموفق با یک خطا
        /// </summary>
        public static PolicyResult Deny(string message, string errorCode)
        {
            return Deny(new PolicyError(message, errorCode));
        }

        /// <summary>
        /// ترکیب چندین نتیجه Policy به صورت AND
        /// - اگر هر یک از سیاست‌ها نقض شود، نتیجه ناموفق است
        /// - تمام خطاهای نقض شده را جمع‌آوری می‌کند
        /// </summary>
        public static PolicyResult Combine(params PolicyResult[] results)
        {
            if (results == null || results.Length == 0)
                return Allow();

            var allErrors = new List<PolicyError>();
            foreach (var result in results)
            {
                if (!result.IsAllowed)
                    allErrors.AddRange(result.Errors);
            }

            return allErrors.Count > 0 ? Deny(allErrors.ToArray()) : Allow();
        }

        /// <summary>
        /// ترکیب چندین نتیجه Policy به صورت AND
        /// - نسخه با پارامتر IEnumerable برای استفاده آسان‌تر
        /// </summary>
        public static PolicyResult Combine(IEnumerable<PolicyResult> results)
        {
            return Combine(results?.ToArray() ?? new PolicyResult[0]);
        }

        /// <summary>
        /// آیا این نتیجه حاوی خطا با کد مشخص است؟
        /// </summary>
        public bool HasError(string errorCode)
        {
            return Errors.Any(e => e.ErrorCode == errorCode);
        }

        /// <summary>
        /// دریافت خطاهای با کد مشخص
        /// </summary>
        public IEnumerable<PolicyError> GetErrorsByCode(string errorCode)
        {
            return Errors.Where(e => e.ErrorCode == errorCode);
        }

        /// <summary>
        /// تبدیل به رشته برای لاگ و دیباگ
        /// </summary>
        public override string ToString()
        {
            return IsAllowed
                ? "PolicyResult: Allowed"
                : $"PolicyResult: Denied ({Errors.Count} errors)";
        }
    }

    /// <summary>
    /// خطای مرتبط با سیاست
    /// </summary>
    public sealed class PolicyError
    {
        /// <summary>
        /// پیام خطا
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// کد خطا
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// سازنده
        /// </summary>
        public PolicyError(string message, string errorCode)
        {
            Message = string.IsNullOrWhiteSpace(message)
                ? "خطا در ارزیابی سیاست"
                : message;
            ErrorCode = string.IsNullOrWhiteSpace(errorCode)
                ? "POLICY_ERROR"
                : errorCode;
        }

        /// <summary>
        /// تبدیل به رشته برای نمایش
        /// </summary>
        public override string ToString()
        {
            return $"[{ErrorCode}] {Message}";
        }
    }
}