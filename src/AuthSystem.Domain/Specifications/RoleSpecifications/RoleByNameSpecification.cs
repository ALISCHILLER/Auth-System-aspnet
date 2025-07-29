using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.RoleSpecifications;

/// <summary>
/// Specification برای فیلتر کردن نقش‌ها بر اساس نام
/// </summary>
public class RoleByNameSpecification : Specification<Role>
{
    private readonly string _name;

    public RoleByNameSpecification(string name)
    {
        _name = name;
    }

    public override Expression<Func<Role, bool>> ToExpression()
    {
        return role => role.Name.ToLower() == _name.ToLower();
    }
}