using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;

namespace AuthSystem.Application.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword)
    : IRequest, ITransactionalRequest;