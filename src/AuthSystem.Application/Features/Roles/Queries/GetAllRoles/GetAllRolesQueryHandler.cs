using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Contracts.Roles;
using MediatR;

namespace AuthSystem.Application.Features.Roles.Queries.GetAllRoles;

public sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetAllRolesQuery, GetAllRolesResponse>
{
    public async Task<GetAllRolesResponse> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        // In a production implementation this would project via the repository.
        var roles = await roleRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return new GetAllRolesResponse
        {
            Roles = roles
                .Select(role => new GetAllRolesResponse.RoleSummary
                {
                    Id = role.Id,
                    Name = role.Name
                })
                .ToArray()
        };
    }
}