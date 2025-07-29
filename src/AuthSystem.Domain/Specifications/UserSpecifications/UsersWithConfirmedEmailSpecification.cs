using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.UserSpecifications;

/// <summary>
/// Specification برای فیلتر کردن کاربرانی که ایمیل آن‌ها تأیید شده است
/// </summary>
public class UsersWithConfirmedEmailSpecification : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.EmailConfirmed;
    }
}
