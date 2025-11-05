using System;
using MediatR;
using AuthSystem.Application.Contracts.Users;

namespace AuthSystem.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<GetUserByIdResponse>;