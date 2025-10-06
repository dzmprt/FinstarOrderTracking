using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FOT.WebApi.SwaggerFilters;

/// <summary>
/// Filer for set required for all not nullable properties (for js clients)
/// </summary>
internal sealed class NonNullableSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null || !context.Type.IsClass)
        {
            return;
        }

        foreach (var property in schema.Properties)
        {
            var cSharpProperty = context.Type.GetProperty(property.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (cSharpProperty != null)
            {
                var isNonNullable = cSharpProperty.PropertyType.IsValueType && Nullable.GetUnderlyingType(cSharpProperty.PropertyType) == null ||
                                    (cSharpProperty.PropertyType.IsClass && cSharpProperty.CustomAttributes.All(a => a.AttributeType.Name != "NullableAttribute")); // Heuristic for NRT

                if (isNonNullable && !schema.Required.Contains(property.Key))
                {
                    schema.Required.Add(property.Key);
                }
            }
        }
    }
}
