using AuthSystem.Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthSystem.Infrastructure.Services.JwtService;

/// <summary>
/// پیاده‌سازی ITokenGenerator برای تولید توکن‌های JWT
/// </summary>
public class JwtService : ITokenGenerator
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="secretKey">کلید مخفی برای امضای توکن</param>
    /// <param name="issuer">صادرکننده توکن</param>
    /// <param name="audience">مخاطب توکن</param>
    /// <param name="accessTokenExpirationMinutes">مدت زمان انقضا برای توکن دسترسی (دقیقه)</param>
    /// <param name="refreshTokenExpirationDays">مدت زمان انقضا برای توکن تازه‌سازی (روز)</param>
    public JwtService(
        string secretKey,
        string issuer,
        string audience,
        int accessTokenExpirationMinutes = 15,
        int refreshTokenExpirationDays = 7)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
        _accessTokenExpirationMinutes = accessTokenExpirationMinutes;
        _refreshTokenExpirationDays = refreshTokenExpirationDays;
    }

    /// <summary>
    /// تولید یک توکن دسترسی (Access Token)
    /// </summary>
    /// <param name="claims">کلیم‌های مورد نیاز</param>
    /// <returns>توکن دسترسی</returns>
    public string GenerateAccessToken(Dictionary<string, object> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims.Select(c => new Claim(c.Key, c.Value.ToString())),
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// تولید یک توکن تازه‌سازی (Refresh Token)
    /// </summary>
    /// <returns>توکن تازه‌سازی</returns>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    /// <summary>
    /// تولید همزمان توکن دسترسی و تازه‌سازی
    /// </summary>
    /// <param name="claims">کلیم‌های مورد نیاز</param>
    /// <returns>توکن دسترسی و تازه‌سازی</returns>
    public (string AccessToken, string RefreshToken) GenerateTokens(Dictionary<string, object> claims)
    {
        var accessToken = GenerateAccessToken(claims);
        var refreshToken = GenerateRefreshToken();
        return (accessToken, refreshToken);
    }
}