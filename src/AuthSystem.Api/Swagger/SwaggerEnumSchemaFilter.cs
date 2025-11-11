using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthSystem.Api.Swagger;

/// <summary>
/// Adds human-readable enum metadata to the generated schema definitions.
/// </summary>
public sealed class SwaggerEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        var names = Enum.GetNames(context.Type);
        var values = Enum.GetValues(context.Type).Cast<object>().Select(Convert.ToInt64).ToArray();

        schema.Description = (schema.Description ?? string.Empty) +
            $" Allowed values: {string.Join(", ", names.Select((name, index) => $"{name} ({values[index]})"))}.";

        var enumNames = new OpenApiArray();
        foreach (var name in names)
        {
            enumNames.Add(new OpenApiString(name));
        }

        schema.Extensions["x-enumNames"] = enumNames;
    }
}