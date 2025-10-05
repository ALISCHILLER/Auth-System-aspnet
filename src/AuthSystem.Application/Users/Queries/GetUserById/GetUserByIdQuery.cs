using System;
using MediatR;
using AuthSystem.Application.Users.Queries.GetUserById.Contracts;

namespace AuthSystem.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<GetUserByIdResponse>;