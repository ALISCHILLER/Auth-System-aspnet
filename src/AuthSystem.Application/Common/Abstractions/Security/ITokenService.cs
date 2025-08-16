namespace AuthSystem.Application.Common.Abstractions.Security;

/// <summary>
/// سرویس تولید و بررسی توکن JWT
/// </summary>
public interface ITokenService
{
    string GenerateAccessToken(string userId, IEnumerable<string> roles, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
}
