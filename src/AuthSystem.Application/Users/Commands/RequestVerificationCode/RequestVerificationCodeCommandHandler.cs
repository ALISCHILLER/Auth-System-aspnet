using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Users.Commands.RequestVerificationCode.Contracts;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Users.Commands.RequestVerificationCode;

public sealed class RequestVerificationCodeCommandHandler(
    IUserRepository userRepository,
    IVerificationCodeService verificationCodeService,
    IEmailSender? emailSender = null,
    ISmsSender? smsSender = null)
    : IRequestHandler<RequestVerificationCodeCommand, RequestVerificationCodeResponse>
{
    public async Task<RequestVerificationCodeResponse> Handle(RequestVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        string code = request.CodeType switch
        {
            VerificationCodeType.EmailVerification or
            VerificationCodeType.AccountActivation or
            VerificationCodeType.EmailChange or
            VerificationCodeType.PasswordReset or
            VerificationCodeType.Transaction or
            VerificationCodeType.NewDevice => await verificationCodeService.GenerateEmailVerificationCodeAsync(user.Id, cancellationToken),
            VerificationCodeType.TwoFactorAuth => await verificationCodeService.GenerateTwoFactorCodeAsync(user.Id, cancellationToken),
            VerificationCodeType.PhoneVerification or
            VerificationCodeType.PhoneChange => await verificationCodeService.GenerateTwoFactorCodeAsync(user.Id, cancellationToken),
            _ => throw InvalidVerificationCodeException.ForInvalidCode()
        };

        if (emailSender is not null && user.Email is not null && request.CodeType is VerificationCodeType.EmailVerification or VerificationCodeType.AccountActivation or VerificationCodeType.EmailChange or VerificationCodeType.PasswordReset or VerificationCodeType.Transaction or VerificationCodeType.NewDevice)
        {
            await emailSender.SendAsync(user.Email.Value, "Verification Code", $"<p>کد تایید شما: <strong>{code}</strong></p>", cancellationToken);
        }

        if (smsSender is not null && user.PhoneNumber is not null && request.CodeType is VerificationCodeType.PhoneVerification or VerificationCodeType.PhoneChange)
        {
            await smsSender.SendAsync(user.PhoneNumber.Value, $"کد تایید شما: {code}", cancellationToken);
        }

        return new RequestVerificationCodeResponse(user.Id, request.CodeType, code);
    }
}