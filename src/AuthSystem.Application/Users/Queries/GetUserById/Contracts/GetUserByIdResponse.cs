using System;

namespace AuthSystem.Application.Users.Queries.GetUserById.Contracts;

public sealed record GetUserByIdResponse(Guid UserId, string Email, string FullName);