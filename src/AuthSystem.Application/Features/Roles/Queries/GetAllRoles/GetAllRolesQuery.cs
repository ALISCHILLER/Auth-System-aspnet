using AuthSystem.Application.Contracts.Roles;
using MediatR;

namespace AuthSystem.Application.Features.Roles.Queries.GetAllRoles;

public sealed record GetAllRolesQuery() : IRequest<GetAllRolesResponse>;