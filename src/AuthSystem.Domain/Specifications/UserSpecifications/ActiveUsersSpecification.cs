using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.UserSpecifications;

/// <summary>
/// Specification برای فیلتر کردن کاربران فعال
/// </summary>
public class ActiveUsersSpecification : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.IsActive;
    }
}