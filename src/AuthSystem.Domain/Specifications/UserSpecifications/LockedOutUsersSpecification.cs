using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.UserSpecifications;

/// <summary>
/// Specification برای فیلتر کردن کاربرانی که حساب آن‌ها قفل شده است
/// </summary>
public class LockedOutUsersSpecification : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow;
    }
}