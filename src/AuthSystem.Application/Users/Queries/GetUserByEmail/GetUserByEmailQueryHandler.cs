using System.Linq;
using MediatR;
using AuthSystem.Application.Common.Interfaces;
using AuthSystem.Application.Users.Queries.GetUserByEmail.Contracts;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Users.Queries.GetUserByEmail;

public sealed class GetUserByEmailQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse>
{
    public Task<GetUserByEmailResponse> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var dto = dbContext.Users
            .Where(user => user.Email != null && user.Email.Value == request.Email)
            .Select(user => new GetUserByEmailResponse(user.Id, user.Email!.Value, user.FullName, user.Status))
            .FirstOrDefault();

        if (dto is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        return Task.FromResult(dto);
    }
}