using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.UserSpecifications;

/// <summary>
/// Specification برای فیلتر کردن کاربران بر اساس نام کاربری
/// </summary>
public class UserByUsernameSpecification : Specification<User>
{
    private readonly string _username;

    public UserByUsernameSpecification(string username)
    {
        _username = username;
    }

    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.UserName.ToLower() == _username.ToLower();
    }
}