using MediatR;
using AuthSystem.Application.Roles.Queries.GetAllRoles.Contracts;

namespace AuthSystem.Application.Roles.Queries.GetAllRoles;

public sealed record GetAllRolesQuery() : IRequest<GetAllRolesResponse>;