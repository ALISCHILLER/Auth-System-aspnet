namespace AuthSystem.Application.Auth.Commands.RefreshToken.Contracts;

public sealed record RefreshTokenResponse(string AccessToken, string RefreshToken);