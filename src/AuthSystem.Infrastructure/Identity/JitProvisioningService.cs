using AuthSystem.Application.Common.Abstractions.Identity;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.Identity;

internal sealed class JitProvisioningService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<JitProvisioningService> logger) : IJitProvisioningService
{
    public async Task<User?> ProvisionAsync(string email, string? tenantId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var existing = await userRepository.GetByEmailAsync(email, cancellationToken).ConfigureAwait(false);
        if (existing is not null)
        {
            return existing;
        }

        try
        {
            var (firstName, lastName) = DeriveName(email);
            var password = Guid.NewGuid().ToString("N");
            var user = new User(
                Guid.NewGuid(),
                Email.Create(email),
                PasswordHash.CreateFromPlainText(password),
                firstName,
                lastName,
                isEmailVerified: true,
                isSocialLogin: true);

            await userRepository.AddAsync(user, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Provisioned user {Email} via JIT provisioning for tenant {Tenant}", email, tenantId);
            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to JIT provision user {Email}", email);
            return null;
        }
    }

    private static (string FirstName, string LastName) DeriveName(string email)
    {
        var username = email.Split('@')[0];
        var parts = username.Split('.', '-', '_');
        if (parts.Length >= 2)
        {
            return (Capitalize(parts[0]), Capitalize(parts[1]));
        }

        return (Capitalize(username), "User");
    }

    private static string Capitalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "User";
        }

        return char.ToUpperInvariant(value[0]) + value[1..];
    }
}