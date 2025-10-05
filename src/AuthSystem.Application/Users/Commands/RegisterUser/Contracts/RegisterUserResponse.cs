using System;

namespace AuthSystem.Application.Users.Commands.RegisterUser.Contracts;

public sealed record RegisterUserResponse(Guid UserId, string Email, string FullName);