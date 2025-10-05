using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Application.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler(IUserRepository userRepository)
    : IRequestHandler<ChangePasswordCommand>
{
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidUserException("کاربر یافت نشد");
        }

        if (!user.PasswordHash.Verify(request.CurrentPassword))
        {
            throw InvalidUserException.ForInvalidPassword();
        }

        var newHash = PasswordHash.CreateFromPlainText(request.NewPassword);
        user.ChangePassword(newHash);

        await userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}