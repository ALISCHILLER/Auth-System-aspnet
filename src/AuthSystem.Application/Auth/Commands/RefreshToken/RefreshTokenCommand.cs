using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Application.Auth.Commands.RefreshToken.Contracts;

namespace AuthSystem.Application.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(Guid UserId, string RefreshToken)
    : IRequest<RefreshTokenResponse>, ITransactionalRequest;