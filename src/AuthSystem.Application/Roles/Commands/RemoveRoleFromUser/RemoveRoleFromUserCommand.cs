using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Roles.Commands.RemoveRoleFromUser;

public sealed record RemoveRoleFromUserCommand(Guid UserId, Guid RoleId)
    : IRequest, AuthorizationBehavior, ITransactionalRequest
{
    public PermissionType RequiredPermission => PermissionType.RoleAssign;
}