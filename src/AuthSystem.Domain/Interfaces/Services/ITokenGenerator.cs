using System.Collections.Generic;

namespace AuthSystem.Domain.Interfaces.Services;

/// <summary>
/// رابط برای سرویس تولید توکن
/// این رابط روش‌های لازم برای تولید توکن‌های دسترسی و تازه‌سازی را تعریف می‌کند
/// </summary>
public interface ITokenGenerator
{
    /// <summary>
    /// تولید یک توکن دسترسی (Access Token)
    /// </summary>
    /// <param name="claims">کلیم‌های مورد نیاز</param>
    /// <returns>توکن دسترسی</returns>
    string GenerateAccessToken(Dictionary<string, object> claims);

    /// <summary>
    /// تولید یک توکن تازه‌سازی (Refresh Token)
    /// </summary>
    /// <returns>توکن تازه‌سازی</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// تولید همزمان توکن دسترسی و تازه‌سازی
    /// </summary>
    /// <param name="claims">کلیم‌های مورد نیاز</param>
    /// <returns>توکن دسترسی و تازه‌سازی</returns>
    (string AccessToken, string RefreshToken) GenerateTokens(Dictionary<string, object> claims);
}