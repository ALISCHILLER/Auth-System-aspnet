using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.Authorization.Role.Rules;

/// <summary>
/// قانون جلوگیری از انتساب تکراری کاربر به نقش
/// </summary>
public sealed class RoleCannotHaveDuplicateUsersRule : BusinessRuleBase
{
    private readonly IEnumerable<Guid> _userIds;
    private readonly Guid _userId;

    public RoleCannotHaveDuplicateUsersRule(IEnumerable<Guid> userIds, Guid userId)
    {
        _userIds = userIds ?? Array.Empty<Guid>();
        _userId = userId;
    }

    public override string Message => "کاربر قبلاً به این نقش منتسب شده است";

    public override bool IsBroken()
    {
        return _userIds.Contains(_userId);
    }
}