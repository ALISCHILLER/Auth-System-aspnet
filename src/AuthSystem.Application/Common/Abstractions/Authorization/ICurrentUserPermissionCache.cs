using System;
using System.Collections.Generic;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Common.Abstractions.Authorization;

public interface ICurrentUserPermissionCache
{
    bool TryGet(Guid userId, out IReadOnlySet<PermissionType> permissions);

    void Set(Guid userId, IReadOnlySet<PermissionType> permissions);
}