using System.Linq;
using MediatR;
using AuthSystem.Application.Common.Interfaces;
using AuthSystem.Application.Users.Queries.GetUserById.Contracts;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = dbContext.Users
            .Where(user => user.Id == request.UserId)
            .Select(user => new GetUserByIdResponse(user.Id, user.Email?.Value ?? string.Empty, user.FullName))
            .FirstOrDefault();

        if (dto is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        return Task.FromResult(dto);
    }
}