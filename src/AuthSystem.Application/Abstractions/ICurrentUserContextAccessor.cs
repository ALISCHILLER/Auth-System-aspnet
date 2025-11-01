using System;
using System.Collections.Generic;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Abstractions;

public interface ICurrentUserContextAccessor
{
    void SetCurrentUser(Guid? userId, IEnumerable<PermissionType> permissions);
}