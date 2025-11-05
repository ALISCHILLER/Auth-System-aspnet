using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Infrastructure.Persistence.Sql;
using AuthSystem.Infrastructure.Verification.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Infrastructure.Verification;

internal sealed class SqlVerificationCodeService(ApplicationDbContext dbContext) : IVerificationCodeService
{
    public async Task<string> IssueAsync(Guid userId, TimeSpan timeToLive, CancellationToken cancellationToken)
    {
        var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var hash = Hash(code);

        var entity = new VerificationCode
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CodeHash = hash,
            ExpiresAtUtc = DateTime.UtcNow.Add(timeToLive)
        };

        await dbContext.VerificationCodes.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return code;
    }

    public async Task<bool> ValidateAsync(Guid userId, string code, CancellationToken cancellationToken)
    {
        var hash = Hash(code);
        var entity = await dbContext.VerificationCodes
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CodeHash == hash && x.ConsumedAtUtc == null, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null || entity.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return false;
        }

        entity.ConsumedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    public async Task InvalidateAsync(Guid userId, string code, CancellationToken cancellationToken)
    {
        var hash = Hash(code);
        var entity = await dbContext.VerificationCodes
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CodeHash == hash, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return;
        }

        entity.ConsumedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static string Hash(string code)
    {
        var bytes = Encoding.UTF8.GetBytes(code);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToHexString(hashBytes);
    }
}