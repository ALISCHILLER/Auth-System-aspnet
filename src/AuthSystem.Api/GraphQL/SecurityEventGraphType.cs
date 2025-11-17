using AuthSystem.Shared.Contracts.Security;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Api.GraphQL;

public sealed class SecurityEventGraphType : ObjectType<SecurityEventDto>
{
    protected override void Configure(IObjectTypeDescriptor<SecurityEventDto> descriptor)
    {
        descriptor.Name("SecurityEvent");
        descriptor.Field(x => x.Id).Type<NonNullType<UuidType>>();
        descriptor.Field(x => x.EventType).Type<NonNullType<EnumType<SecurityEventTypeEnumType>>>();
        descriptor.Field(x => x.UserId).Type<UuidType>();
        descriptor.Field(x => x.UserName).Type<StringType>();
        descriptor.Field(x => x.TenantId).Type<StringType>();
        descriptor.Field(x => x.OccurredAtUtc).Type<NonNullType<DateTimeType>>();
        descriptor.Field(x => x.IpAddress).Type<StringType>();
        descriptor.Field(x => x.UserAgent).Type<StringType>();
        descriptor.Field(x => x.Description).Type<StringType>();
        descriptor.Field(x => x.Metadata)
        .Type<ListType<NonNullType<SecurityEventMetadataEntryType>>>()
        .Resolve(context => ConvertMetadata(context.Parent<SecurityEventDto>().Metadata));
    }

    private static IReadOnlyList<SecurityEventMetadataEntry> ConvertMetadata(
        IReadOnlyDictionary<string, string>? metadata)
    {
        if (metadata is null || metadata.Count == 0)
        {
            return Array.Empty<SecurityEventMetadataEntry>();
        }

        return metadata
            .Select(pair => new SecurityEventMetadataEntry(pair.Key, pair.Value))
            .ToList();
    }
}

public sealed class SecurityEventTypeEnumType : EnumType<SecurityEventType>
{
}