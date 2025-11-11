using AuthSystem.Domain.Common.Specifications;

namespace AuthSystem.Domain.Entities.UserAggregate.Specifications;

/// <summary>
/// مشخصه جستجوی کاربر بر اساس ایمیل
/// </summary>
public sealed class UserByEmailSpecification : BaseSpecification<User>
{
    public UserByEmailSpecification(string email)
        : base(user => user.Email != null && user.Email.Value == email)
    {
    }
}