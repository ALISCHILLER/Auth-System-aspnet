using AuthSystem.Domain.Common.Specifications;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.UserAggregate.Specifications;

/// <summary>
/// مشخصه کاربران با وضعیت فعال
/// </summary>
public sealed class ActiveUserSpecification : BaseSpecification<User>
{
    public ActiveUserSpecification()
        : base(user => user.Status == UserStatus.Active)
    {
        AddOrderBy(user => user.CreatedAt);
    }
}