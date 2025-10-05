using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;

namespace AuthSystem.Application.Users.Commands.VerifyEmail;

public sealed record VerifyEmailCommand(Guid UserId, string Code)
    : IRequest, ITransactionalRequest;