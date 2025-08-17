using AuthSystem.Domain.Common;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Rules;

/// <summary>
/// قانون: کاربر قفل شده نمی‌تواند وارد شود
/// </summary>
public class UserCannotLoginWhenLockedRule : IBusinessRule
{
    private readonly User _user;

    /// <summary>
    /// سازنده قانون
    /// </summary>
    public UserCannotLoginWhenLockedRule(User user)
    {
        _user = user;
    }

    /// <summary>
    /// پیام خطا در صورت نقض قانون
    /// </summary>
    public string Message => "کاربر قفل شده است و نمی‌تواند وارد شود";

    /// <summary>
    /// کد خطا در صورت نقض قانون
    /// </summary>
    public string ErrorCode => "USER_LOCKED";

    /// <summary>
    /// بررسی نقض قانون
    /// </summary>
    public bool IsBroken()
    {
        return _user.IsLocked;
    }
}