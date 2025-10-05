using System.Linq;
using MediatR;
using AuthSystem.Application.Common.Interfaces;
using AuthSystem.Application.Roles.Queries.GetRoleByName.Contracts;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Roles.Queries.GetRoleByName;

public sealed class GetRoleByNameQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetRoleByNameQuery, GetRoleByNameResponse>
{
    public Task<GetRoleByNameResponse> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        var dto = dbContext.Roles
            .Where(role => role.Name == request.Name)
            .Select(role => new GetRoleByNameResponse(
                role.Id,
                role.Name,
                role.Description,
                role.IsDefault,
                role.IsSystemRole,
                role.Permissions.Select(permission => permission.PermissionType).ToArray()))
            .FirstOrDefault();

        if (dto is null)
        {
            throw InvalidUserRoleException.ForNonExistentRole(request.Name);
        }

        return Task.FromResult(dto);
    }
}