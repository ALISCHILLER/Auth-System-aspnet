using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون منحصر به فرد بودن ایمیل کاربر
/// </summary>
public sealed class UserEmailMustBeUniqueRule : BusinessRuleBase
{
    private readonly string _email;
    private readonly Func<string, bool> _emailExists;

    public UserEmailMustBeUniqueRule(string email, Func<string, bool> emailExists)
    {
        _email = email;
        _emailExists = emailExists ?? throw new ArgumentNullException(nameof(emailExists));
    }

    public override string Message => "ایمیل وارد شده قبلاً ثبت شده است";

    public override bool IsBroken()
    {
        return _emailExists(_email);
    }
}