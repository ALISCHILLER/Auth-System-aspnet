using System.Linq;
using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Auth.Queries.GetCurrentUser.Contracts;
using AuthSystem.Application.Common.Interfaces;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Auth.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    public Task<GetCurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (currentUserService.UserId is null)
        {
            throw new System.UnauthorizedAccessException("کاربر احراز هویت نشده است");
        }

        var projection = dbContext.Users
            .Where(user => user.Id == currentUserService.UserId.Value)
            .Select(user => new
            {
                user.Id,
                Email = user.Email?.Value ?? string.Empty,
                user.FullName,
                Roles = user.Roles.Values.ToArray()
            })
            .FirstOrDefault();

        if (projection is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        return Task.FromResult(new GetCurrentUserResponse(projection.Id, projection.Email, projection.FullName, projection.Roles));
    }
}