using System;
using AuthSystem.Application.Common.Markers;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.Logout;

public sealed record LogoutCommand(
    string RefreshToken,
    string? TenantId,
    string? IpAddress,
    string? UserAgent,
    Guid? UserId = null,
    string? UserName = null) : IRequest<Unit>, ITransactionalRequest;