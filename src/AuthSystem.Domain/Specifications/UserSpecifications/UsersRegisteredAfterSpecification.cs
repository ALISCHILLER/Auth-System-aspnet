using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.UserSpecifications;

/// <summary>
/// Specification برای فیلتر کردن کاربرانی که پس از یک تاریخ خاص ثبت‌نام کرده‌اند
/// </summary>
public class UsersRegisteredAfterSpecification : Specification<User>
{
    private readonly DateTime _date;

    public UsersRegisteredAfterSpecification(DateTime date)
    {
        _date = date;
    }

    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.CreatedAt > _date;
    }
}
