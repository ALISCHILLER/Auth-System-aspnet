using System;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Users.Queries.GetUserByEmail.Contracts;

public sealed record GetUserByEmailResponse(Guid UserId, string Email, string FullName, UserStatus Status);