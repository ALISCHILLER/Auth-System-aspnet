using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Application.Users.Commands.EnableTwoFactor.Contracts;

namespace AuthSystem.Application.Users.Commands.EnableTwoFactor;

public sealed record EnableTwoFactorCommand(Guid UserId, string Issuer, string? Email)
    : IRequest<EnableTwoFactorResponse>, ITransactionalRequest;