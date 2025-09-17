using AuthSystem.Domain.Common.Specifications;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Domain.Entities.UserAggregate.Specifications;

/// <summary>
/// مشخصه جستجوی کاربر بر اساس ایمیل
/// </summary>
public sealed class UserByEmailSpecification : BaseSpecification<User>
{
    public UserByEmailSpecification(string email)
        : base(user => user.Email?.Value == email)
    {
    }
}