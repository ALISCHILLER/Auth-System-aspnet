using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Common.Abstractions.Authorization;

public interface IRequirePermission
{
    PermissionType RequiredPermission { get; }
}