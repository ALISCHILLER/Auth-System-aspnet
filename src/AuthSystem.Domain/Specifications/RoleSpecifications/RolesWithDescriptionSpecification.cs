using AuthSystem.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications.RoleSpecifications;

/// <summary>
/// Specification برای فیلتر کردن نقش‌هایی که توضیحات دارند
/// </summary>
public class RolesWithDescriptionSpecification : Specification<Role>
{
    public override Expression<Func<Role, bool>> ToExpression()
    {
        return role => !string.IsNullOrEmpty(role.Description);
    }
}