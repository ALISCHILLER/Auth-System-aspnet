using AuthSystem.Application.Contracts.Users;
using MediatR;

namespace AuthSystem.Application.Features.Users.Commands.TwoFactor.Request;

public sealed record RequestTwoFactorCodeCommand(Guid UserId, TwoFactorDeliveryChannel Channel) : IRequest<Unit>;