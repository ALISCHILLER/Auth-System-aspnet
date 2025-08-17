using AuthSystem.Domain.Common;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Rules;

/// <summary>
/// قانون: رمز عبور باید از پیچیدگی کافی برخوردار باشد
/// </summary>
public class UserMustHaveValidPasswordRule : IBusinessRule
{
    private readonly Password _password;

    /// <summary>
    /// سازنده قانون
    /// </summary>
    public UserMustHaveValidPasswordRule(Password password)
    {
        _password = password;
    }

    /// <summary>
    /// پیام خطا در صورت نقض قانون
    /// </summary>
    public string Message => "رمز عبور باید از پیچیدگی کافی برخوردار باشد";

    /// <summary>
    /// کد خطا در صورت نقض قانون
    /// </summary>
    public string ErrorCode => "PASSWORD_WEAK";

    /// <summary>
    /// بررسی نقض قانون
    /// </summary>
    public bool IsBroken()
    {
        // در عمل باید از یک سرویس امنیتی برای بررسی پیچیدگی استفاده کنیم
        return _password.Value.Length < 8;
    }
}