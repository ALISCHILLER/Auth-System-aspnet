using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.UserSpecifications;

/// <summary>
/// Specification برای فیلتر کردن کاربرانی که شماره تلفن آن‌ها تأیید شده است
/// </summary>
public class UsersWithConfirmedPhoneNumberSpecification : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.PhoneNumberConfirmed;
    }
}