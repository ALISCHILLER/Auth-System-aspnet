using System;
using MediatR;
using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Markers;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Features.Roles.Commands.AssignRoleToUser;

public sealed record AssignRoleToUserCommand(Guid UserId, Guid RoleId)
    : IRequest<Unit>, ITransactionalRequest, IRequirePermission
{
    public PermissionType RequiredPermission => PermissionType.RoleAssign;
}