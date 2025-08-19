using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Rules;

/// <summary>
/// اینترفیس برای قوانین کسب‌وکار ناهمزمان
/// </summary>
public interface IAsyncBusinessRule : IBusinessRule
{
    /// <summary>
    /// بررسی آیا قانون نقض شده است (ناهمزمان)
    /// </summary>
    Task<bool> IsBrokenAsync();

    /// <summary>
    /// دریافت پیام خطای قانون (ناهمزمان)
    /// </summary>
    Task<string> GetMessageAsync();
}