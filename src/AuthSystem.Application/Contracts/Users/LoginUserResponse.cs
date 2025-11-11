namespace AuthSystem.Application.Contracts.Users;

public sealed class LoginUserResponse
{
    public Guid UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}