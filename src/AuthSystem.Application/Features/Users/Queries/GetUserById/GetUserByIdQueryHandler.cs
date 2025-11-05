using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Application.Contracts.Users;
using MediatR;

namespace AuthSystem.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            throw new NotFoundException($"User '{request.UserId}' was not found.");
        }

        return new GetUserByIdResponse
        {
            Id = user.Id,
            Email = user.Email?.Value ?? string.Empty,
            FullName = $"{user.FirstName} {user.LastName}".Trim()
        };
    }
}