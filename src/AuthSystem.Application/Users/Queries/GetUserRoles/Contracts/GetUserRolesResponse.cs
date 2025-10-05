using System;
using System.Collections.Generic;

namespace AuthSystem.Application.Users.Queries.GetUserRoles.Contracts;

public sealed record GetUserRolesResponse(Guid UserId, string Email, string FullName, IReadOnlyCollection<string> Roles);