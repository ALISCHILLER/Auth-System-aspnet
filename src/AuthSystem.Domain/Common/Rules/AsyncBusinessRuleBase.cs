using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Rules;

/// <summary>
/// کلاس پایه برای قوانین کسب‌وکار ناهمزمان
/// </summary>
public abstract class AsyncBusinessRuleBase : IAsyncBusinessRule
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
    /// بررسی آیا قانون نقض شده است (ناهمزمان)
    /// </summary>
    public abstract Task<bool> IsBrokenAsync();
}