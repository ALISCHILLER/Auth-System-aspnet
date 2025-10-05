using System;
using MediatR;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Domain.Enums;
using AuthSystem.Application.Users.Commands.RegisterUser.Contracts;

namespace AuthSystem.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    DateTime? DateOfBirth
) : IRequest<RegisterUserResponse>, AuthorizationBehavior, ITransactionalRequest
{
    public PermissionType RequiredPermission => PermissionType.UserCreate;
}