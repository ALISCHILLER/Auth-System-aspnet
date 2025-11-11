using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace AuthSystem.Infrastructure.Identity;

internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId => httpContextAccessor.HttpContext?.User.GetUserId();

    public Task<IReadOnlySet<PermissionType>> GetPermissionsAsync(CancellationToken cancellationToken)
    {
        var principal = httpContextAccessor.HttpContext?.User;
        if (principal is null)
        {
            return Task.FromResult<IReadOnlySet<PermissionType>>(new HashSet<PermissionType>());
        }

        var permissions = principal.GetPermissions();
        return Task.FromResult<IReadOnlySet<PermissionType>>(permissions);
    }
}