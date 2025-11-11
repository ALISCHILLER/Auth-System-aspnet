using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Common.Abstractions.Security;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Task<IReadOnlySet<PermissionType>> GetPermissionsAsync(CancellationToken cancellationToken);
}