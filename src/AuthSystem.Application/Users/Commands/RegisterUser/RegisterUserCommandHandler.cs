using System;
using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Users.Commands.RegisterUser.Contracts;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IEmailSender? emailSender = null)
    : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        var existingUser = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingUser is not null)
        {
            throw DuplicateEmailException.ForEmail(email.Value);
        }

        var passwordHash = PasswordHash.CreateFromPlainText(request.Password);
        var phoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : PhoneNumber.Create(request.PhoneNumber!);

        var user = new User(
            Guid.NewGuid(),
            email,
            passwordHash,
            request.FirstName,
            request.LastName,
            phoneNumber,
            request.DateOfBirth);

        await userRepository.AddAsync(user, cancellationToken);

        if (emailSender is not null)
        {
            await emailSender.SendAsync(email.Value, "Welcome to AuthSystem", $"<p>سلام {user.FullName}!</p>", cancellationToken);
        }

        return new RegisterUserResponse(user.Id, user.Email?.Value ?? string.Empty, user.FullName);
    }
}