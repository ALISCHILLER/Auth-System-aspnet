using MediatR;
using AuthSystem.Application.Users.Queries.GetUserByEmail.Contracts;

namespace AuthSystem.Application.Users.Queries.GetUserByEmail;

public sealed record GetUserByEmailQuery(string Email) : IRequest<GetUserByEmailResponse>;