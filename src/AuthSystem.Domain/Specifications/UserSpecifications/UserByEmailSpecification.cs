using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.UserSpecifications;

/// <summary>
/// Specification برای فیلتر کردن کاربران بر اساس آدرس ایمیل
/// </summary>
public class UserByEmailSpecification : Specification<User>
{
    private readonly string _email;

    public UserByEmailSpecification(string email)
    {
        _email = email;
    }

    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.EmailAddress.ToLower() == _email.ToLower();
    }
}