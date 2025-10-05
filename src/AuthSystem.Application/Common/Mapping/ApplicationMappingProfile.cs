using System.Linq;
using AutoMapper;
using AuthSystem.Application.Users.Commands.RegisterUser.Contracts;
using AuthSystem.Application.Users.Queries.GetUserByEmail.Contracts;
using AuthSystem.Application.Users.Queries.GetUserById.Contracts;
using AuthSystem.Application.Users.Queries.GetUserRoles.Contracts;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Application.Common.Mapping;

public sealed class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<User, RegisterUserResponse>()
            .ConstructUsing(user => new RegisterUserResponse(user.Id, user.Email?.Value ?? string.Empty, user.FullName));

        CreateMap<User, GetUserByEmailResponse>()
            .ConstructUsing(user => new GetUserByEmailResponse(user.Id, user.Email?.Value ?? string.Empty, user.FullName, user.Status));

        CreateMap<User, GetUserByIdResponse>()
            .ConstructUsing(user => new GetUserByIdResponse(user.Id, user.Email?.Value ?? string.Empty, user.FullName));

        CreateMap<User, GetUserRolesResponse>()
            .ConstructUsing(user => new GetUserRolesResponse(user.Id, user.Email?.Value ?? string.Empty, user.FullName, user.Roles.Values.ToArray()));
    }
}