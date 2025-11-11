using AuthSystem.Application.Common.Markers;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.Logout;

public sealed record LogoutCommand(
    string RefreshToken,
    string? TenantId,
    string? IpAddress,
    string? UserAgent) : IRequest<Unit>, ITransactionalRequest;