using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;

namespace AuthSystem.Application.Tokens.Commands.RevokeToken;

public sealed record RevokeTokenCommand(Guid UserId, string RefreshToken)
    : IRequest, ITransactionalRequest;