using AuthSystem.Domain.Exceptions.Infrastructure;
using AuthSystem.Domain.Interfaces.Services;
using AuthSystem.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthSystem.Infrastructure.Services.JwtService;

/// <summary>
/// پیاده‌سازی ITokenGenerator با استفاده از JWT
/// این کلاس برای تولید توکن‌های دسترسی و تازه‌سازی استفاده می‌شود
/// </summary>
public class JwtService : ITokenGenerator
{
    private readonly JwtOptions _options;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="options">تنظیمات JWT</param>
    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        // اعتبارسنجی تنظیمات
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(_options);

        if (!Validator.TryValidateObject(_options, validationContext, validationResults, true))
        {
            var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
            throw new InvalidConfigurationException("Jwt", errors);
        }
    }

    /// <summary>
    /// تولید یک توکن دسترسی (Access Token)
    /// </summary>
    /// <param name="claims">کلیم‌های مورد نیاز</param>
    /// <returns>توکن دسترسی</returns>
    public string GenerateAccessToken(Dictionary<string, object> claims)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenClaims = new List<Claim>();
            foreach (var claim in claims)
            {
                tokenClaims.Add(new Claim(claim.Key, claim.Value.ToString()!));
            }

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: tokenClaims,
                expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (SecurityTokenException ex)
        {
            throw new InfrastructureException("خطا در تولید توکن دسترسی.", ex);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطای ناشناخته در تولید توکن دسترسی.", ex);
        }
    }

    /// <summary>
    /// تولید یک توکن تازه‌سازی (Refresh Token)
    /// </summary>
    /// <returns>توکن تازه‌سازی</returns>
    public string GenerateRefreshToken()
    {
        try
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در تولید توکن تازه‌سازی.", ex);
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