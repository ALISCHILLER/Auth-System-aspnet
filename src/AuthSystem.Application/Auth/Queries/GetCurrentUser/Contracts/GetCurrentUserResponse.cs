using System;
using System.Collections.Generic;

namespace AuthSystem.Application.Auth.Queries.GetCurrentUser.Contracts;

public sealed record GetCurrentUserResponse(Guid UserId, string Email, string FullName, IReadOnlyCollection<string> Roles);