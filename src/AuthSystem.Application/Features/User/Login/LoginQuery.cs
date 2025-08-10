using MediatR;
using AuthSystem.Application.DTOs.Responses;
using AuthSystem.Application.Common;

namespace AuthSystem.Application.Features.User.Login;

/// <summary>
/// پرس‌وجو ورود به سیستم
/// </summary>
public class LoginQuery : IRequest<Result<LoginResponse>>
{
    public string UsernameOrEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? TwoFactorCode { get; set; }
    public bool RememberMe { get; set; }
    public string? ClientInfo { get; set; }
}