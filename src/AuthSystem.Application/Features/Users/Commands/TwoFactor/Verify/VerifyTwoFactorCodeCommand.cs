using System;
using AuthSystem.Application.Contracts.Users;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.TwoFactor.Verify;

public sealed record VerifyTwoFactorCodeCommand(Guid UserId, string Code, string? TenantId, string? IpAddress, string? UserAgent)
    : IRequest<TwoFactorVerificationResponse>;