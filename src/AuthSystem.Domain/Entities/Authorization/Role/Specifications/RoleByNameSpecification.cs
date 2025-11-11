using AuthSystem.Domain.Common.Specifications;

namespace AuthSystem.Domain.Entities.Authorization.Role.Specifications;

/// <summary>
/// مشخصه جستجوی نقش بر اساس نام
/// </summary>
public sealed class RoleByNameSpecification : BaseSpecification<Role>
{
    public RoleByNameSpecification(string name)
        : base(role => role.Name == name)
    {
        AddOrderBy(role => role.Name);
    }
}