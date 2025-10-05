using System;
using MediatR;
using AuthSystem.Application.Tokens.Queries.ValidateToken.Contracts;

namespace AuthSystem.Application.Tokens.Queries.ValidateToken;

public sealed record ValidateTokenQuery(string Token, bool IsAccessToken, Guid? UserId) : IRequest<ValidateTokenResponse>;