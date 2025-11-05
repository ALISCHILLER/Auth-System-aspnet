using MediatR;
using AuthSystem.Application.Contracts.Users;

namespace AuthSystem.Application.Features.Users.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResponse>;