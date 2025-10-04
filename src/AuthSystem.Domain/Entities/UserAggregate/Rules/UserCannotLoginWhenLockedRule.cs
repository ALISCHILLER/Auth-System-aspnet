using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// Business rule preventing login when the user account is locked.
/// </summary>
public sealed class UserCannotLoginWhenLockedRule : BusinessRuleBase
{
    private readonly DateTime? _lockoutEnd;

    public UserCannotLoginWhenLockedRule(DateTime? lockoutEnd)
    {
        _lockoutEnd = lockoutEnd;
    }

    public override string Message => "حساب کاربر قفل است";

    public override bool IsBroken()
    {
        return _lockoutEnd.HasValue && _lockoutEnd.Value > DomainClock.Instance.UtcNow;
    }
}