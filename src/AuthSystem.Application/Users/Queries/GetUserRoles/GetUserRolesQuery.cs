using System;
using MediatR;
using AuthSystem.Application.Users.Queries.GetUserRoles.Contracts;

namespace AuthSystem.Application.Users.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(Guid UserId) : IRequest<GetUserRolesResponse>;