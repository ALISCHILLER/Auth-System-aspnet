namespace AuthSystem.Domain.Common.Rules;

/// <summary>
/// کلاس پایه برای قوانین کسب‌وکار
/// </summary>
public abstract class BusinessRuleBase : IBusinessRule
{
    /// <summary>
    /// پیام خطای قانون
    /// </summary>
    public abstract string Message { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public virtual string ErrorCode => GetType().Name;

    /// <summary>
    /// بررسی آیا قانون نقض شده است
    /// </summary>
    public abstract bool IsBroken();
}