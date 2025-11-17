namespace AuthSystem.Api.GraphQL;

/// <summary>
/// Represents a metadata key/value pair exposed to the GraphQL schema.
/// </summary>
public sealed record SecurityEventMetadataEntry(string Key, string? Value);