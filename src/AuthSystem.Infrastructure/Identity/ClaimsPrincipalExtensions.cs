using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Infrastructure.Identity;

internal static class ClaimsPrincipalExtensions
{
    private const string PermissionClaimType = "perm";

    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            return null;
        }

        var idValue = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(idValue, out var id) ? id : null;
    }

    public static IReadOnlySet<PermissionType> GetPermissions(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            return new HashSet<PermissionType>();
        }

        var permissions = new HashSet<PermissionType>();
        foreach (var claim in principal.FindAll(PermissionClaimType))
        {
            if (Enum.TryParse<PermissionType>(claim.Value, ignoreCase: true, out var parsed))
            {
                permissions.Add(parsed);
                continue;
            }

            if (Enum.TryParse<PermissionType>(claim.Value.Replace(":", string.Empty), true, out parsed))
            {
                permissions.Add(parsed);
            }
        }

        return permissions;
    }
}