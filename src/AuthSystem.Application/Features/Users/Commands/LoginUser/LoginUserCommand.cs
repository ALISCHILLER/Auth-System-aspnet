using AuthSystem.Application.Contracts.Users;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password,
    string? TenantId,
    string? IpAddress,
    string? UserAgent,
    bool ExternalLogin = false) : IRequest<LoginUserResponse>;