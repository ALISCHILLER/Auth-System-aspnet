using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای مدیریت توکن JWT
/// </summary>
public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims, string? ipAddress = null, string? userAgent = null);
    string GenerateRefreshToken(Guid userId, string? ipAddress = null, string? userAgent = null);
    ClaimsPrincipal? ValidateToken(string token);
    bool IsTokenValid(string token);
    bool IsRefreshTokenValid(string token);
    DateTime? GetTokenExpiration(string token);
}