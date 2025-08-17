using System.Threading.Tasks;

namespace AuthSystem.Domain.Common;

/// <summary>
/// رابط برای قوانین کسب‌وکار به صورت async
/// این رابط برای قوانینی استفاده می‌شود که نیاز به I/O دارند
/// </summary>
public interface IAsyncBusinessRule
{
    /// <summary>
    /// پیام خطا در صورت نقض قانون
    /// </summary>
    string Message { get; }

    /// <summary>
    /// کد خطا برای شناسایی در API
    /// </summary>
    string ErrorCode { get; }

    /// <summary>
    /// بررسی نقض قانون به صورت async
    /// </summary>
    /// <returns>true اگر قانون نقض شده باشد</returns>
    Task<bool> IsBrokenAsync();
}

/// <summary>
/// رابط پایه برای قوانین کسب‌وکار ساده (sync)
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// پیام خطا در صورت نقض قانون
    /// </summary>
    string Message { get; }

    /// <summary>
    /// کد خطا برای شناسایی در API
    /// </summary>
    string ErrorCode { get; }

    /// <summary>
    /// بررسی نقض قانون
    /// </summary>
    /// <returns>true اگر قانون نقض شده باشد</returns>
    bool IsBroken();
}

/// <summary>
/// کلاس پایه برای قوانین کسب‌وکار با پیاده‌سازی پیش‌فرض
/// </summary>
public abstract class BusinessRuleBase : IBusinessRule
{
    public abstract string Message { get; }
    public virtual string ErrorCode => GetType().Name.Replace("Rule", "");
    public abstract bool IsBroken();
}

/// <summary>
/// کلاس پایه برای قوانین کسب‌وکار async
/// </summary>
public abstract class AsyncBusinessRuleBase : IAsyncBusinessRule
{
    public abstract string Message { get; }
    public virtual string ErrorCode => GetType().Name.Replace("Rule", "");
    public abstract Task<bool> IsBrokenAsync();
}
