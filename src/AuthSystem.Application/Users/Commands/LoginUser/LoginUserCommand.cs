using MediatR;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Application.Users.Commands.LoginUser.Contracts;

namespace AuthSystem.Application.Users.Commands.LoginUser;

public sealed record LoginUserCommand(
    string EmailOrUsername,
    string Password,
    string? IpAddress,
    string? UserAgent,
    bool RememberMe
) : IRequest<LoginUserResponse>, ITransactionalRequest;