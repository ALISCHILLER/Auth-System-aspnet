using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Exceptions;

namespace AuthSystem.Application.Users.Commands.VerifyEmail;

public sealed class VerifyEmailCommandHandler(
    IUserRepository userRepository,
    IVerificationCodeService verificationCodeService)
    : IRequestHandler<VerifyEmailCommand>
{
    public async Task<Unit> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        var isValid = await verificationCodeService.ValidateEmailVerificationCodeAsync(user.Id, request.Code, cancellationToken);
        if (!isValid)
        {
            throw InvalidVerificationCodeException.ForInvalidCode();
        }

        user.VerifyEmail();

        await userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}