using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Common.Authorization;

internal sealed class CurrentUserPermissionCache : ICurrentUserPermissionCache
{
    private readonly Dictionary<Guid, IReadOnlySet<PermissionType>> _cache = new();

    public bool TryGet(Guid userId, out IReadOnlySet<PermissionType> permissions)
        => _cache.TryGetValue(userId, out permissions!);

    public void Set(Guid userId, IReadOnlySet<PermissionType> permissions)
    {
        _cache[userId] = permissions;
    }
}