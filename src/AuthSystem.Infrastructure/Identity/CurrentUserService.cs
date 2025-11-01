using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Infrastructure.Identity;

internal sealed class CurrentUserService(CurrentUserContext context) : ICurrentUserService, ICurrentUserContextAccessor
{
    public Guid? UserId => context.UserId;

    public Task<bool> HasPermissionAsync(PermissionType permission, CancellationToken ct)
    {
        var hasPermission = context.Permissions.Contains(permission);
        return Task.FromResult(hasPermission);
    }

    public void SetCurrentUser(Guid? userId, IEnumerable<PermissionType> permissions)
        => context.SetUser(userId, permissions);
}