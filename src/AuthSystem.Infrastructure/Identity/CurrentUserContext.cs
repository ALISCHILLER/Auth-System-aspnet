using System;
using System.Collections.Generic;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Infrastructure.Identity;

internal sealed class CurrentUserContext
{
    private readonly HashSet<PermissionType> _permissions = new();

    public Guid? UserId { get; private set; }

    public IReadOnlyCollection<PermissionType> Permissions => _permissions;

    public void SetUser(Guid? userId, IEnumerable<PermissionType> permissions)
    {
        UserId = userId;
        _permissions.Clear();
        if (permissions is null)
        {
            return;
        }

        foreach (var permission in permissions)
        {
            _permissions.Add(permission);
        }
    }
}