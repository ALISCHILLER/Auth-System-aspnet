using AuthSystem.Application.Common.Abstractions.Messaging;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Application.Contracts.Users;
using FluentValidation.Results;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.TwoFactor.Request;

public sealed class RequestTwoFactorCodeCommandHandler(
    IUserRepository userRepository,
    IVerificationCodeService verificationCodeService,
    IEmailSender emailSender,
    ISmsSender smsSender)
    : IRequestHandler<RequestTwoFactorCodeCommand, Unit>
{
    public async Task<Unit> Handle(RequestTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"User '{request.UserId}' was not found");

        var code = await verificationCodeService
            .IssueAsync(user.Id, TimeSpan.FromMinutes(5), cancellationToken)
            .ConfigureAwait(false);

        switch (request.Channel)
        {
            case TwoFactorDeliveryChannel.Email:
                if (user.Email is null)
                {
                    throw new AppValidationException(new[]
                    {
                        new ValidationFailure(nameof(user.Email), "User does not have a verified email address.")
                    });
                }

                await emailSender.SendAsync(
                    user.Email.Value,
                    "Two-factor verification code",
                    $"<p>Your verification code is <strong>{code}</strong>. It expires in 5 minutes.</p>",
                    cancellationToken).ConfigureAwait(false);
                break;

            case TwoFactorDeliveryChannel.Sms:
                if (user.PhoneNumber is null)
                {
                    throw new AppValidationException(new[]
                    {
                        new ValidationFailure(nameof(user.PhoneNumber), "User does not have a phone number on record.")
                    });
                }

                await smsSender.SendAsync(
                    user.PhoneNumber.Value,
                    $"Your verification code is {code}. It expires in 5 minutes.",
                    cancellationToken).ConfigureAwait(false);
                break;

            default:
                throw new AppValidationException(new[]
                {
                    new ValidationFailure(nameof(request.Channel), "Unsupported delivery channel.")
                });
        }

        return Unit.Value;
    }
}