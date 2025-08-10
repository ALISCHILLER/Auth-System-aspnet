using AuthSystem.Application.Common;
using MediatR;

namespace AuthSystem.Application.Features.User.ChangePassword;

/// <summary>
/// دستور تغییر رمز عبور
/// </summary>
public class ChangePasswordCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmNewPassword { get; set; } = null!;
}