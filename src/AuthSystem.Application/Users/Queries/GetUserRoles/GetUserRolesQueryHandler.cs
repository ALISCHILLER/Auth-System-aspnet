using System.Linq;
using MediatR;
using AuthSystem.Application.Common.Interfaces;
using AuthSystem.Application.Users.Queries.GetUserRoles.Contracts;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Users.Queries.GetUserRoles;

public sealed class GetUserRolesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetUserRolesQuery, GetUserRolesResponse>
{
    public Task<GetUserRolesResponse> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userProjection = dbContext.Users
            .Where(user => user.Id == request.UserId)
            .Select(user => new
            {
                user.Id,
                Email = user.Email?.Value ?? string.Empty,
                user.FullName,
                Roles = user.Roles.Values.ToArray()
            })
            .FirstOrDefault();

        if (userProjection is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        return Task.FromResult(new GetUserRolesResponse(userProjection.Id, userProjection.Email, userProjection.FullName, userProjection.Roles));
    }
}