using AuthSystem.Domain.Common.Rules;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون اعتبارسنجی نام و نام خانوادگی کاربر
/// </summary>
public sealed class UserMustHaveValidNameRule : BusinessRuleBase
{
    private static readonly Regex NamePattern = new(@"^[\p{L} \-']+$", RegexOptions.Compiled);
    private readonly string _firstName;
    private readonly string _lastName;

    public UserMustHaveValidNameRule(string firstName, string lastName)
    {
        _firstName = firstName;
        _lastName = lastName;
    }

    public override string Message => "نام و نام خانوادگی کاربر معتبر نیست";

    public override bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(_firstName)
            || string.IsNullOrWhiteSpace(_lastName)
            || _firstName.Length is < 2 or > 50
            || _lastName.Length is < 2 or > 100
            || !NamePattern.IsMatch(_firstName)
            || !NamePattern.IsMatch(_lastName);
    }
}