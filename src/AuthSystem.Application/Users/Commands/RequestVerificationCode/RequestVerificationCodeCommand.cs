using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Application.Users.Commands.RequestVerificationCode.Contracts;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Users.Commands.RequestVerificationCode;

public sealed record RequestVerificationCodeCommand(
    Guid UserId,
    VerificationCodeType CodeType
) : IRequest<RequestVerificationCodeResponse>, ITransactionalRequest;