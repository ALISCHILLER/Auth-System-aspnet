using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.ValueObjects;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email, cancellationToken).ConfigureAwait(false);
        if (existingUser is not null)
        {
            throw new ConflictException("Email already registered.");
        }

        var passwordHash = PasswordHash.CreateFromPlainText(request.Password);
        PhoneNumber? phoneNumber = null;
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        }

        var user = new User(
            Guid.NewGuid(),
            Email.Create(request.Email),
            passwordHash,
            request.FirstName,
            request.LastName,
            phoneNumber);

        await userRepository.AddAsync(user, cancellationToken).ConfigureAwait(false);

        return new RegisterUserResponse
        {
            Id = user.Id,
            Email = user.Email?.Value ?? request.Email,
            FullName = $"{user.FirstName} {user.LastName}".Trim()
        };
    }
}