using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Abstractions;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Task<bool> HasPermissionAsync(PermissionType permission, CancellationToken ct);
}