using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Enums;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.UserSpecifications;

/// <summary>
/// Specification برای فیلتر کردن کاربران بر اساس جنسیت
/// </summary>
public class UsersByGenderSpecification : Specification<User>
{
    private readonly Gender _gender;

    public UsersByGenderSpecification(Gender gender)
    {
        _gender = gender;
    }

    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.Gender == _gender;
    }
}