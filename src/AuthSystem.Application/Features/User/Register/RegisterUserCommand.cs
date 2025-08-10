using AuthSystem.Application.Common;
using MediatR;

namespace AuthSystem.Application.Features.User.Register;

/// <summary>
/// دستور ثبت‌نام کاربر جدید
/// </summary>
public class RegisterUserCommand : IRequest<Result>
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? NationalCode { get; set; }
    public string? Culture { get; set; }
    public string? ProfileImage { get; set; }
}