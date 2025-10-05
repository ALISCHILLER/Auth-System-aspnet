using System;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Users.Commands.RequestVerificationCode.Contracts;

public sealed record RequestVerificationCodeResponse(Guid UserId, VerificationCodeType CodeType, string Code);