namespace AuthSystem.Application.Users.Commands.EnableTwoFactor.Contracts;

public sealed record EnableTwoFactorResponse(string SecretKey, string QrCodeUri);