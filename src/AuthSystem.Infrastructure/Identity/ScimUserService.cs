using AuthSystem.Application.Common.Abstractions.Identity;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.ValueObjects;
using AuthSystem.Infrastructure.Persistence.Sql;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace AuthSystem.Infrastructure.Identity;

internal sealed class ScimUserService(ApplicationDbContext dbContext) : IScimUserService
{
    public async Task<IReadOnlyCollection<ScimUserRepresentation>> SearchAsync(int startIndex, int count, CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, startIndex - 1);
        var take = Math.Clamp(count, 1, 200);

        var users = await dbContext.Users
            .AsNoTracking()
            .OrderBy(user => user.FirstName)
            .ThenBy(user => user.LastName)
            .Skip(skip)
            .Take(take)
            .Select(MapToRepresentation)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return users;
    }

    public async Task<ScimUserRepresentation?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var userId))
        {
            return null;
        }

        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
        return user is null ? null : MapToRepresentation(user);
    }

    public async Task<ScimUserRepresentation> CreateAsync(ScimUserResource resource, CancellationToken cancellationToken)
    {
        var (firstName, lastName) = ResolveName(resource);
        var email = string.IsNullOrWhiteSpace(resource.Email) ? null : Email.Create(resource.Email);
        var password = PasswordHash.CreateFromPlainText(Guid.NewGuid().ToString("N"));

        var user = new User(Guid.NewGuid(), email, password, firstName, lastName, isEmailVerified: !string.IsNullOrWhiteSpace(resource.Email), isSocialLogin: false);

        if (!string.IsNullOrWhiteSpace(resource.UserName))
        {
            user.SetUsername(resource.UserName);
        }

        user.SetStatus(resource.Active ? UserStatus.Active : UserStatus.Inactive);

        await dbContext.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToRepresentation(user);
    }

    public async Task<ScimUserRepresentation?> ReplaceAsync(string id, ScimUserResource resource, CancellationToken cancellationToken)
    {
        var user = await FindTrackedUserAsync(id, cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(resource.Email))
        {
            user.ChangeEmail(Email.Create(resource.Email));
            if (resource.Active)
            {
                user.VerifyEmail();
            }
        }

        if (!string.IsNullOrWhiteSpace(resource.UserName))
        {
            user.SetUsername(resource.UserName);
        }

        user.SetStatus(resource.Active ? UserStatus.Active : UserStatus.Inactive);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        dbContext.Entry(user).State = EntityState.Detached;

        return MapToRepresentation(user);
    }

    public async Task<ScimUserRepresentation?> PatchAsync(string id, JsonObject patchRequest, CancellationToken cancellationToken)
    {
        var user = await FindTrackedUserAsync(id, cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return null;
        }

        if (patchRequest["Operations"] is JsonArray operations)
        {
            foreach (var operation in operations.OfType<JsonObject>())
            {
                var op = operation["op"]?.GetValue<string>()?.ToLowerInvariant();
                var path = operation["path"]?.GetValue<string>()?.ToLowerInvariant();

                if (string.Equals(op, "replace", StringComparison.Ordinal))
                {
                    if (string.Equals(path, "active", StringComparison.Ordinal))
                    {
                        var value = ResolveBoolean(operation["value"]);
                        user.SetStatus(value ? UserStatus.Active : UserStatus.Inactive);
                    }
                    else if (string.Equals(path, "username", StringComparison.Ordinal))
                    {
                        var value = operation["value"]?.GetValue<string>();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            user.SetUsername(value);
                        }
                    }
                    else if (string.Equals(path, "emails", StringComparison.Ordinal) && operation["value"] is JsonArray emailArray)
                    {
                        var primaryEmail = emailArray.OfType<JsonObject>().FirstOrDefault()?.GetValue<string>("value");
                        if (!string.IsNullOrWhiteSpace(primaryEmail))
                        {
                            user.ChangeEmail(Email.Create(primaryEmail));
                        }
                    }
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        dbContext.Entry(user).State = EntityState.Detached;

        return MapToRepresentation(user);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var user = await FindTrackedUserAsync(id, cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return false;
        }

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    private async Task<User?> FindTrackedUserAsync(string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var userId))
        {
            return null;
        }

        return await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
    }

    private static ScimUserRepresentation MapToRepresentation(User user)
        => new()
        {
            Id = user.Id.ToString(),
            UserName = user.Username ?? user.Email?.Value ?? user.Id.ToString(),
            DisplayName = user.FullName,
            Email = user.Email?.Value,
            Active = user.Status == UserStatus.Active,
            CreatedAtUtc = user.LastLoginAt,
            Roles = Array.Empty<string>()
        };

    private static (string FirstName, string LastName) ResolveName(ScimUserResource resource)
    {
        var firstName = !string.IsNullOrWhiteSpace(resource.GivenName) ? resource.GivenName.Trim() : null;
        var lastName = !string.IsNullOrWhiteSpace(resource.FamilyName) ? resource.FamilyName.Trim() : null;

        if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
        {
            return (firstName!, lastName!);
        }

        if (!string.IsNullOrWhiteSpace(resource.UserName))
        {
            var segments = resource.UserName.Split(new[] { '.', '-', '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length >= 2)
            {
                return (Capitalize(segments[0]), Capitalize(segments[1]));
            }

            return (Capitalize(resource.UserName), "User");
        }

        if (!string.IsNullOrWhiteSpace(resource.Email))
        {
            var userNamePart = resource.Email.Split('@')[0];
            var segments = userNamePart.Split(new[] { '.', '-', '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length >= 2)
            {
                return (Capitalize(segments[0]), Capitalize(segments[1]));
            }

            return (Capitalize(userNamePart), "User");
        }

        return ("Scim", "User");
    }

    private static string Capitalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "User";
        }

        return char.ToUpperInvariant(value[0]) + value[1..];
    }

    private static bool ResolveBoolean(JsonNode? value)
    {
        if (value is null)
        {
            return false;
        }

        return value switch
        {
            JsonValue jsonValue when jsonValue.TryGetValue<bool>(out var result) => result,
            JsonValue jsonValue when jsonValue.TryGetValue<string>(out var text) && bool.TryParse(text, out var parsed) => parsed,
            _ => false
        };
    }
}