using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.RoleSpecifications;

/// <summary>
/// Specification برای فیلتر کردن نقش‌ها بر اساس شناسه
/// </summary>
public class RoleByIdSpecification : Specification<Role>
{
    private readonly Guid _roleId;

    public RoleByIdSpecification(Guid roleId)
    {
        _roleId = roleId;
    }

    public override Expression<Func<Role, bool>> ToExpression()
    {
        return role => role.Id == _roleId;
    }
}