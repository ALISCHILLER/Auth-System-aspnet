using HotChocolate.Types;

namespace AuthSystem.Api.GraphQL;

public sealed class SecurityEventMetadataEntryType : ObjectType<SecurityEventMetadataEntry>
{
    protected override void Configure(IObjectTypeDescriptor<SecurityEventMetadataEntry> descriptor)
    {
        descriptor.Name("SecurityEventMetadataEntry");
        descriptor.Field(x => x.Key).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Value).Type<StringType>();
    }
}