namespace AuthSystem.Application.Common.Abstractions.Security;

/// <summary>
/// سرویس دریافت اطلاعات کاربر جاری
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
    string? Username { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
}
