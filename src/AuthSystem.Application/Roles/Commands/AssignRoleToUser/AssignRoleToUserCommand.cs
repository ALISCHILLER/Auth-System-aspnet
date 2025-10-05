using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Roles.Commands.AssignRoleToUser;

public sealed record AssignRoleToUserCommand(Guid UserId, Guid RoleId)
    : IRequest, AuthorizationBehavior, ITransactionalRequest
{
    public PermissionType RequiredPermission => PermissionType.RoleAssign;
}