using System.Linq;
using MediatR;
using AuthSystem.Application.Common.Interfaces;
using AuthSystem.Application.Roles.Queries.GetAllRoles.Contracts;

namespace AuthSystem.Application.Roles.Queries.GetAllRoles;

public sealed class GetAllRolesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetAllRolesQuery, GetAllRolesResponse>
{
    public Task<GetAllRolesResponse> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = dbContext.Roles
            .Select(role => new RoleSummary(
                role.Id,
                role.Name,
                role.Description,
                role.IsDefault,
                role.IsSystemRole,
                role.Permissions.Select(permission => permission.PermissionType).ToArray()))
            .ToArray();

        return Task.FromResult(new GetAllRolesResponse(roles));
    }
}