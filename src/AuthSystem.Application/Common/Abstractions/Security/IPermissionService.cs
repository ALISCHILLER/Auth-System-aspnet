namespace AuthSystem.Application.Common.Abstractions.Security;

/// <summary>
/// سرویس بررسی مجوزهای کاربر
/// </summary>
public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string permission, CancellationToken cancellationToken = default);
}
