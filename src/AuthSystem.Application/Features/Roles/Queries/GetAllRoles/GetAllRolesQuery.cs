using MediatR;
using AuthSystem.Application.Contracts.Roles;

namespace AuthSystem.Application.Features.Roles.Queries.GetAllRoles;

public sealed record GetAllRolesQuery() : IRequest<GetAllRolesResponse>;