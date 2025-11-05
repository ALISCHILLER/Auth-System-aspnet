using AutoMapper;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Domain.Entities.UserAggregate;

namespace AuthSystem.Application.Common.Mapping;

public sealed class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<User, RegisterUserResponse>()
            .ForMember(destination => destination.Email, options => options.MapFrom(source => source.Email != null ? source.Email.Value : string.Empty))
            .ForMember(destination => destination.FullName, options => options.MapFrom(source => $"{source.FirstName} {source.LastName}".Trim()));
    }
}