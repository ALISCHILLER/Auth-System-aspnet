using AuthSystem.Application.Common;
using MediatR;

namespace AuthSystem.Application.Features.User.ConfirmEmail;

/// <summary>
/// دستور تأیید ایمیل
/// </summary>
public class ConfirmEmailCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
}