using AuthSystem.Domain.Common.Rules;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.Entities.Authorization.Role.Rules;

/// <summary>
/// قانون اعتبارسنجی نام نقش
/// </summary>
public sealed class RoleNameMustBeValidRule : BusinessRuleBase
{
    private static readonly Regex NamePattern = new(@"^[A-Za-z0-9_-]{3,50}$", RegexOptions.Compiled);
    private readonly string _name;

    public RoleNameMustBeValidRule(string name)
    {
        _name = name;
    }

    public override string Message => "نام نقش باید بین 3 تا 50 کاراکتر و شامل حروف انگلیسی، عدد، خط تیره یا زیرخط باشد";

    public override bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(_name) || !NamePattern.IsMatch(_name);
    }
}