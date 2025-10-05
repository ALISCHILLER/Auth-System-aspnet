using MediatR;
using AuthSystem.Application.Auth.Queries.GetCurrentUser.Contracts;

namespace AuthSystem.Application.Auth.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery() : IRequest<GetCurrentUserResponse>;