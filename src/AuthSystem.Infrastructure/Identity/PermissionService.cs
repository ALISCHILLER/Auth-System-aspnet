using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Domain.Enums;
using AuthSystem.Infrastructure.Persistence.Sql;
using Microsoft.EntityFrameworkCore;
namespace AuthSystem.Infrastructure.Identity;

internal sealed class PermissionService(ApplicationDbContext dbContext) : IPermissionService
{
    public async Task<IReadOnlyCollection<PermissionType>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var permissions = await (from assignment in dbContext.UserRoles
                                 join permission in dbContext.RolePermissions on assignment.RoleId equals permission.RoleId
                                 where assignment.UserId == userId
                                 select permission.PermissionType)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return permissions;
    }
}