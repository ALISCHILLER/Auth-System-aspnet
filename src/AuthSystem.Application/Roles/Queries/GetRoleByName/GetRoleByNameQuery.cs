using MediatR;
using AuthSystem.Application.Roles.Queries.GetRoleByName.Contracts;

namespace AuthSystem.Application.Roles.Queries.GetRoleByName;

public sealed record GetRoleByNameQuery(string Name) : IRequest<GetRoleByNameResponse>;