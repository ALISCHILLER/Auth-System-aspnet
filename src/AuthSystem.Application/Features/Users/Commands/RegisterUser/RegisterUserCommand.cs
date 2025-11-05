using MediatR;
using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Markers;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber)
    : IRequest<RegisterUserResponse>, ITransactionalRequest, IRequirePermission
{
    public PermissionType RequiredPermission => PermissionType.UserCreate;
}