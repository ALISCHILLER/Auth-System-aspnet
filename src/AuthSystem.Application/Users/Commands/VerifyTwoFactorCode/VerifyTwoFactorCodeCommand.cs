using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Application.Users.Commands.LoginUser.Contracts;

namespace AuthSystem.Application.Users.Commands.VerifyTwoFactorCode;

public sealed record VerifyTwoFactorCodeCommand(
    Guid UserId,
    string Code,
    string? IpAddress,
    string? UserAgent
) : IRequest<LoginUserResponse>, ITransactionalRequest;