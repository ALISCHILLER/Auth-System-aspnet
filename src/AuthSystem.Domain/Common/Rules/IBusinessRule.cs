namespace AuthSystem.Domain.Common.Rules;

/// <summary>
/// اینترفیس پایه برای تمام قوانین کسب‌وکار
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// پیام خطای قانون
    /// </summary>
    string Message { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    string ErrorCode { get; }

    /// <summary>
    /// بررسی آیا قانون نقض شده است
    /// </summary>
    bool IsBroken();
}