using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Abstractions;

public interface IVerificationCodeService
{
    Task<string> GenerateEmailVerificationCodeAsync(Guid userId, CancellationToken ct);
    Task<bool> ValidateEmailVerificationCodeAsync(Guid userId, string code, CancellationToken ct);
    Task<string> GenerateTwoFactorCodeAsync(Guid userId, CancellationToken ct);
    Task<bool> ValidateTwoFactorCodeAsync(Guid userId, string code, CancellationToken ct);
}