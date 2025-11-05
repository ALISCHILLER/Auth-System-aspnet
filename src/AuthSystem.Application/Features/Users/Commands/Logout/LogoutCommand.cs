using MediatR;
using AuthSystem.Application.Common.Markers;

namespace AuthSystem.Application.Features.Users.Commands.Logout;

public sealed record LogoutCommand(string RefreshToken) : IRequest<Unit>, ITransactionalRequest;