using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Common.Abstractions.Authorization;

public interface IPermissionService
{
    Task<IReadOnlySet<PermissionType>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken);
}