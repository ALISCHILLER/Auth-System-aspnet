using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;

namespace AuthSystem.Application.Auth.Commands.Logout;

public sealed record LogoutCommand(Guid UserId, string? RefreshToken)
    : IRequest, ITransactionalRequest;