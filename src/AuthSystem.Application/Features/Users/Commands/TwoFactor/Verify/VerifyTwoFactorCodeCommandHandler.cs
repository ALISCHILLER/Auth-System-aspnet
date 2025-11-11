using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Application.Common.Models;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Shared.Contracts.Security;
using FluentValidation.Results;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.TwoFactor.Verify;

public sealed class VerifyTwoFactorCodeCommandHandler(
    IUserRepository userRepository,
    IVerificationCodeService verificationCodeService,
    ISecurityEventPublisher securityEventPublisher)
    : IRequestHandler<VerifyTwoFactorCodeCommand, TwoFactorVerificationResponse>
{
    public async Task<TwoFactorVerificationResponse> Handle(VerifyTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"User '{request.UserId}' was not found");

        var isValid = await verificationCodeService
            .ValidateAsync(user.Id, request.Code, cancellationToken)
            .ConfigureAwait(false);

        if (!isValid)
        {
            var metadata = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(request.TenantId))
            {
                metadata["tenantId"] = request.TenantId!;
            }

            metadata["reason"] = "Invalid or expired verification code";

            await securityEventPublisher.PublishAsync(
                new SecurityEventContext(
                    SecurityEventType.TwoFactorFailed,
                    user.Id,
                    user.FullName,
                    request.TenantId,
                    request.IpAddress,
                    request.UserAgent,
                    "Two-factor verification failed.",
                    metadata),
                cancellationToken).ConfigureAwait(false);

            throw new AppValidationException(new[]
            {
                new ValidationFailure(nameof(request.Code), "The provided verification code is invalid or expired.")
            });
        }

        await verificationCodeService.InvalidateAsync(user.Id, request.Code, cancellationToken).ConfigureAwait(false);

        return new TwoFactorVerificationResponse
        {
            Succeeded = true
        };
    }
}