using System.Text.RegularExpressions;
using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون اعتبارسنجی نام کاربری
/// </summary>
public sealed class UsernameMustBeValidRule : BusinessRuleBase
{
    private static readonly Regex UsernamePattern = new(@"^[A-Za-z0-9_.-]{3,32}$", RegexOptions.Compiled);
    private readonly string _username;

    public UsernameMustBeValidRule(string username)
    {
        _username = username;
    }

    public override string Message => "نام کاربری باید بین 3 تا 32 کاراکتر و شامل حروف انگلیسی، عدد یا نمادهای _.- باشد";

    public override bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(_username) || !UsernamePattern.IsMatch(_username);
    }
}