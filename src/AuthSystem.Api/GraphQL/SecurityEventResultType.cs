using AuthSystem.Shared.Contracts.Security;
using AuthSystem.Shared.DTOs;
using HotChocolate.Types;

namespace AuthSystem.Api.GraphQL;

public sealed class SecurityEventResultType : ObjectType<PagedResult<SecurityEventDto>>
{
    protected override void Configure(IObjectTypeDescriptor<PagedResult<SecurityEventDto>> descriptor)
    {
        descriptor.Name("SecurityEventConnection");
        descriptor.Field(x => x.Items).Type<NonNullType<ListType<NonNullType<SecurityEventGraphType>>>>();
        descriptor.Field(x => x.PageNumber).Type<NonNullType<IntType>>();
        descriptor.Field(x => x.PageSize).Type<NonNullType<IntType>>();
        descriptor.Field(x => x.TotalCount).Type<NonNullType<IntType>>();
        descriptor.Field(x => x.TotalPages).Type<NonNullType<IntType>>();
    }
}