using AuthSystem.Domain.Common.Specifications;

namespace AuthSystem.Domain.Entities.UserAggregate.Specifications;

/// <summary>
/// مشخصه جستجوی کاربر بر اساس نام کاربری
/// </summary>
public sealed class UserByUsernameSpecification : BaseSpecification<User>
{
    public UserByUsernameSpecification(string username)
        : base(user => user.Username == username)
    {
    }
}