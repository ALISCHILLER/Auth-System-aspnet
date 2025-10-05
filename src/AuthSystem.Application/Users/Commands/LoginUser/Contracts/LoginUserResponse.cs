using System;

namespace AuthSystem.Application.Users.Commands.LoginUser.Contracts;

public sealed record LoginUserResponse(
    Guid UserId,
    string AccessToken,
    string RefreshToken,
    bool RequiresTwoFactor);