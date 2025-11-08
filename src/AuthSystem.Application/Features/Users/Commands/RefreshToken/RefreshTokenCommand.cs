using AuthSystem.Application.Contracts.Users;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<LoginUserResponse>;